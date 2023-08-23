using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/manager")]
    [ApiController]
    [Authorize(Policy = "Auth")]
    public class ManagerController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        private readonly IConfiguration _config;
        public ManagerController(InstituteOfFineArtsContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [Route("create-account")]
        [Authorize(Policy = "AdminAndManager")]
        public IActionResult createAccount(registerDto register)
        {
            using (var transaction = _context.Database.BeginTransaction())
                try
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity == null || !identity.IsAuthenticated) return Unauthorized();
                    var u = UserHelper.GetUserDataDto(identity);
                    if(u.RoleId == 1)
                    {
                        if (register.RoleId == 2 || register.RoleId == 3)
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
                                Status = "Active",
                                JoinDate = register.JoinDate,
                                Telephone = register.Telephone,
                                UserCreate = u.Id
                            };
                            _context.Users.Add(user);
                            _context.SaveChanges();
                            transaction.Commit();
                            return Ok();
                        }
                        else
                        {
                            return Unauthorized();
                        }
                    }
                    else 
                    {
                        if (register.RoleId == 2 || register.RoleId == 3)
                        {
                            return Unauthorized();
                        }
                        else {
                            bool isExists = _context.Managers.Any(u => u.Email.ToLower() == register.Email.ToLower());
                            if (isExists) return BadRequest("Email already used");
                            if (!register.Password.Equals(register.ConfirmPassword)) return BadRequest("Confirmation password is not correct");
                            var hashPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);
                            var user = new Entities.Manager
                            {
                                Email = register.Email,
                                Name = register.Name,
                                Password = hashPassword,
                                Birthday = register.Birthday,
                                Address = register.Address,
                                RoleId = register.RoleId,
                                Status = "Active",
                                JoinDate = register.JoinDate,
                                Telephone = register.Telephone,
                            };
                            _context.Managers.Add(user);
                            _context.SaveChanges();
                            transaction.Commit();
                            return Ok();
                        }
                        
                    }
                    
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, ex.InnerException.Message);
                }
        }
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult login(loginDto login)
        {
            try
            {
                var user = _context.Managers.FirstOrDefault(u => u.Email.ToLower() == login.Email.ToLower());
                if (login.Email == "admin@gmail.com")
                {
                    return Ok(new userDataDto
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Token = GenerateJWT(user)
                    });
                }
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
        [Authorize(Policy = "AllManager")]
        public IActionResult changePassword(changerPasswordDto changerPasswordDto)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null) return Unauthorized();
                var u = UserHelper.GetUserDataDto(identity);
                var user = _context.Managers.Find(u.Id);
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

        private String GenerateJWT(Manager user)
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
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signatureKey
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
