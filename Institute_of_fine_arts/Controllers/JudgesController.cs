using Institute_of_fine_arts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/[controller]")]
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
        public IActionResult Index(int id)
        {
            try
            {
                var data = _context.Judges
                    .Include(j => j.Teacher)
                    .Where(j => j.CompetitionId == id)
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
