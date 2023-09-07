using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Claims;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/evaluates")]
    [ApiController]
    public class EvaluatesController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        public EvaluatesController(InstituteOfFineArtsContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Policy = "Teacher")]
        public IActionResult create(CreateEvaluatesDto createEvaluates)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                if (user == null) return Unauthorized();
                var judges = _context.Judges.FirstOrDefault(j =>
                j.CompetitionId == createEvaluates.CompetitionId &&
                j.TeacherId1 == user.Id || j.TeacherId2 == user.Id || j.TeacherId3 == user.Id || j.TeacherId4 == user.Id);
                if (judges == null) return Unauthorized();
                var art = _context.Arts.FirstOrDefault(x => x.Slug == createEvaluates.ArtSlug);
                if (art == null) return BadRequest("Art not found");
                var e = _context.Evaluates.FirstOrDefault(e => e.ArtsId == art.Id && e.TeacherId == user.Id);
                if (e == null)
                {
                    var evaluate = new Entities.Evaluate
                    {
                        Layout = createEvaluates.Layout,
                        Content = createEvaluates.Content,
                        Color = createEvaluates.Color,
                        Creative = createEvaluates.Creative,
                        Feedback = createEvaluates.FeedBack,
                        ArtsId = art.Id,
                        TeacherId = user.Id,
                        CreatedAt = createEvaluates.CreatedAt,
                        Status = "Done",
                        Total = (createEvaluates.Layout + createEvaluates.Content + createEvaluates.Color + createEvaluates.Creative) / 4,
                    };
                    _context.Evaluates.Add(evaluate);
                    art.Granded = 1;
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    e.Layout = createEvaluates.Layout;
                    e.Content = createEvaluates.Content;
                    e.Color = createEvaluates.Color;
                    e.Creative = createEvaluates.Creative;
                    e.Feedback = createEvaluates.FeedBack;
                    e.UpdatedAt = DateTime.Now;
                    e.Total = (createEvaluates.Layout + createEvaluates.Content + createEvaluates.Color + createEvaluates.Creative) / 4;
                    _context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get-one")]
        public IActionResult getOne(string slug)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                if (user == null) return Unauthorized();
                var art = _context.Arts.FirstOrDefault(a => a.Slug == slug);
                if (art == null) return BadRequest("Art not found");
                var evaluate = _context.Evaluates.FirstOrDefault(e => e.ArtsId == art.Id && e.TeacherId == user.Id);
                if (evaluate == null) return NotFound();
                return Ok(evaluate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get-by-art-slug")]
        public IActionResult getByArtSlug(string slug)
        {
            try
            {

                var art = _context.Arts.FirstOrDefault(a => a.Slug == slug);
                if (art == null) return BadRequest("Art not found");
                var evaluate = _context.Evaluates.Where(e => e.ArtsId == art.Id).Include(x => x.Teacher).ToList();

                if (evaluate == null) return NotFound();
                return Ok(evaluate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
