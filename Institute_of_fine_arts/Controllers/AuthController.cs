using Institute_of_fine_arts.Entities;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Institute_of_fine_arts.Dto;
using System.ComponentModel.DataAnnotations;
using Institute_of_fine_arts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Institute_of_fine_arts.Controllers
{
    
    [ApiController]
    [Route("api/auth")]
    [Authorize(Policy = "Auth")]

    public class AuthController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        private readonly IConfiguration _config;
        public AuthController(InstituteOfFineArtsContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult login(loginDto login)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == login.Email.ToLower());
                if (user != null && user.Status == "Block") return BadRequest("This account has been locked, please contact the administrator");
                if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                {
                    return Ok(new userDataDto
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Token = GenerateJWT(user)
                    });
                }
                return BadRequest("email or password is incorrect");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        
        [HttpPost]
        [Route("change-password")]
        public IActionResult changePassword(changerPasswordDto changerPasswordDto)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null) return Unauthorized();
                var u = UserHelper.GetUserDataDto(identity);
                var user = _context.Users.Find(u.Id);
                if (BCrypt.Net.BCrypt.Verify(changerPasswordDto.OldPassword, user.Password))
                {
                    if (changerPasswordDto.NewPassword.Equals(changerPasswordDto.ConfirmPassword))
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(changerPasswordDto.NewPassword);
                        return Ok();
                    }
                    return BadRequest("Confirmation password is not correct");
                }
                return BadRequest("Password is not correct");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("profile")]
        public IActionResult getProfile()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                var u = _context.Users.FirstOrDefault(x=>x.Id == user.Id);
                    return Ok(u);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private String GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signatureKey = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.RoleId.ToString()),
            };
            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signatureKey
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult logout()
        {
            return Ok(true);
        }
    }
}

