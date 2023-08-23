using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IO;
namespace Institute_of_fine_arts.Controllers
{
    [Route("api/art")]
    [ApiController]
    [Authorize(Policy = "Auth")]
    public class ArtsController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        public ArtsController(InstituteOfFineArtsContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("upload")]
        [Authorize(Policy = "Student")]
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
                    Path = path+"/"+fileName ,
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
        [Authorize(Policy = "Student")]
        public IActionResult createArt(createArtDto createArt)
        {
            using (var transaction = _context.Database.BeginTransaction())
                try
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity == null || !identity.IsAuthenticated) return Unauthorized();
                    var user = UserHelper.GetUserDataDto(identity);
                    if (user == null) return Unauthorized();

                    var art = new Entities.Art
                    {
                        Name = createArt.Name,
                        Slug = createArt.Slug,
                        IsSell = createArt.IsSell,
                        Price = createArt.Price,
                        Description = createArt.Description,
                        Url = createArt.Url,
                        Path = createArt.Path,
                        Status = "Submitedd",
                        CompetitionId = createArt.CompetitionId,
                        OwnerId = user.Id.Value,
                        CreatedAt = DateTime.Now,
                    };
                    _context.Arts.Add(art);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, ex.Message);
                }
        }
        [HttpPut]
        [Route("update")]
        [Authorize(Policy ="Student")]
        public IActionResult updateArt(updateArtDto updateArt)
        {
            using(var transaction = _context.Database.BeginTransaction())
            try
            {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    var user = UserHelper.GetUserDataDto(identity);
                    var a = _context.Arts
                        .FirstOrDefault(a=>
                        a.OwnerId == user.Id && 
                        a.CompetitionId== updateArt.CompetitionId);
                    if (a != null)
                    {
                        if (updateArt.Url == null && updateArt.Path == null)
                        {
                            a.Name = updateArt.Name ?? a.Name;
                            a.Slug = updateArt.Slug ?? a.Slug;
                            a.Description = updateArt.Description ?? a.Description;
                            a.IsSell = updateArt.IsSell != null ? updateArt.IsSell : a.IsSell;
                            a.Price = updateArt.Price != null ? updateArt.Price : a.Price;
                            a.UpdatedAt = DateTime.Now;
                        }
                        else
                        {
                            if (a.Url != null)
                            {
                                DeleteImage(a.Url);
                            }
                            a.Name = updateArt.Name ?? a.Name;
                            a.Slug = updateArt.Slug ?? a.Slug;
                            a.Description = updateArt.Description ?? a.Description;
                            a.IsSell = updateArt.IsSell != null  ? updateArt.IsSell: a.IsSell;
                            a.Price = updateArt.Price != null ? updateArt.Price: a.Price;
                            a.Url = updateArt.Url;
                            a.Path = updateArt.Path;
                            a.UpdatedAt = DateTime.Now;
                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        return NoContent();
                    }
                    else
                    {
                        return NotFound("Art not found.");
                    }
                }
                catch (Exception ex)
            {
                    transaction.Rollback();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult getAllArtIsPrize()
        {
            try
            {
                var data = _context.Arts.Where(a=>a.PrizeId != null).ToList();
                if (data.Count < 1) return NotFound();
                return Ok(data);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private void DeleteImage(string imageUrl)
        {
            try
            {
                if (System.IO.File.Exists(imageUrl))
                {
                    System.IO.File.Delete(imageUrl);
                    Console.WriteLine("Image deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Image not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }
    }
}
