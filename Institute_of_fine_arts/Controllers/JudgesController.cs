using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/judges")]
    [ApiController]
    public class JudgesController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        public JudgesController(InstituteOfFineArtsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("teacher")]
        public IActionResult Index(string slug)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                if (user == null) return Unauthorized();
                var competition = _context.Competitions.FirstOrDefault(c => c.Slug == slug);
                if (competition == null) return BadRequest();
                var data = _context.Judges
                    .Include(j => j.Teacher)
                    .Where(j => j.CompetitionId == competition.Id)
                    .ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
