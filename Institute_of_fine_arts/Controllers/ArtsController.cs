using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/art")]
    [ApiController]
    public class ArtsController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        public ArtsController(InstituteOfFineArtsContext context)
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
                var path = "wwwroot/uploads";
                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(image.FileName);
                var upload = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
                image.CopyTo(new FileStream(upload, FileMode.Create));
                var rs = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                var response = new uploadDto
                {
                    Rs = rs,
                    Path = path + "/" + fileName
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public IActionResult create(createArtDto createArt)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var user = UserHelper.GetUserDataDto(identity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
