using Institute_of_fine_arts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Institute_of_fine_arts.Dto;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        public UsersController(InstituteOfFineArtsContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("upload")]
        public IActionResult uploadImage(IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest("No image uploaded");
                }
                if (image.ContentType != "image/jpeg")
                {
                    return BadRequest("Invalid image type");
                }
                var path = "wwwroot/uploads/avartar";
                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(image.FileName);
                var upload = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
                image.CopyTo(new FileStream(upload, FileMode.Create));
                var rs = $"{Request.Scheme}://{Request.Host}/uploads/avartar/{fileName}";
                var response = new uploadDto
                {
                    Rs = rs,
                    Path = path + "/" + fileName,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("top-author")]
        public IActionResult getAuthor()
        {
            try
            {
                var user = _context.Users
                    .Include(u => u.Arts)
                    .Where(u => u.RoleId == 3 && u.Arts.Any(a => a.PrizeId != null))
                    .OrderByDescending(u => u.Arts.Count(a => a.PrizeId != null))
                    .ToList();
                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("by-owner")]
        public IActionResult getByOwnerId(int owner)
        {
            try
            {
                var data = _context.Users.FirstOrDefault(u => u.Id == owner);
                return Ok(data);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
