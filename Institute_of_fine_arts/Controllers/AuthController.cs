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
    [Route("api/auth")]
    [ApiController]
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
        [Route("register")]
        public IActionResult register(registerDto register)
        {
            using (var transaction = _context.Database.BeginTransaction())
                try
                {
                    bool isExists = _context.Users.Any(u => u.Email.ToLower() == register.Email.ToLower());
                    if (isExists) return BadRequest("Email already used");
                    if (!register.Password.Equals(register.ConfirmPassword)) return BadRequest("Confirmation password is not correct");
                    var hashPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);
                    var user = new Entities.User
                    {
                        Email = register.Email,
                        Name = register.Name,
                        Password = hashPassword,
                        Birthday = register.Birthday,
                        Address = register.Address,
                        RoleId = register.RoleId,
                        Status = register.RoleId == 2 ? "Pending" : "Active",
                        JoinDate = register.JoinDate,
                        Tel = register.Telephone,
                    };
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok(new userDataDto
                    {
                        Email = register.Email,
                        Name = register.Name,
                        Token = GenerateJWT(user)
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, ex.InnerException.Message);
                }
        }

        [HttpPost]
        [Route("login")]
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
        [HttpPut]
        public IActionResult approveTeacher([FromRoute] int id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user != null && user.Status == "Pending")
                {
                    user.Status = "Active";
                    _context.SaveChanges();
                    return Ok();
                }
                return BadRequest("Unregistered or activated email");
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

        [HttpDelete]
        public IActionResult delete([FromRoute] int id)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null) return Unauthorized();
                var u = UserHelper.GetUserDataDto(identity);
                var user = _context.Users.Find(id);
                if (user == null) return NotFound();
                user.Status = "Block";
                return NoContent();
            }
            catch (Exception ex)
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
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
            };
            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signatureKey
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

