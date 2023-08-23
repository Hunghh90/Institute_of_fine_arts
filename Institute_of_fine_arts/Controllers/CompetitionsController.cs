using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/competition")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private InstituteOfFineArtsContext _context;
        private Timer _timer;
        public CompetitionsController(InstituteOfFineArtsContext context)
        {
            _context = context;
            _timer = new Timer(UpdateStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        [HttpGet]
        public IActionResult get([FromQuery] string query)
        {
            try
            {
                List<competitionDto> data;
                if (query != null)
                {
                    data = _context.Competitions
                       .Where(data => data.Name.Contains(query))
                       .Select(u => new competitionDto
                       {
                           Name = u.Name,
                           StartDate = u.StartDate,
                           Slug = u.Slug,
                           EndDate = u.EndDate,
                           Description = u.Description,
                           Theme = u.Theme,
                       })
                       .ToList();
                }
                else
                {
                    data = _context.Competitions
                   .Select(u => new competitionDto
                   {
                       Name = u.Name,
                       Slug = u.Slug,
                       StartDate = u.StartDate,
                       EndDate = u.EndDate,
                       Description = u.Description,
                       Theme = u.Theme,
                   })
                   .ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public IActionResult create(createComprtitionDto createComprtition)
        {
            try
            {
                if (createComprtition.StartDate <= DateTime.Now.AddDays(7) || createComprtition.EndDate <= createComprtition.StartDate)
                    return BadRequest("Invalid dates. Start date must be greater than the current time and end date must be greater than the start date.");
                var token = HttpContext.Request.Headers["Authorization"];
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (!identity.IsAuthenticated) 
                    return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                var check = _context.Competitions.FirstOrDefault(c => c.Name.ToLower() == createComprtition.Name.ToLower());
                if (check != null) return BadRequest("Competition is Exists");
                var cp = new Entities.Competition
                {
                    Name = createComprtition.Name,
                    StartDate = createComprtition.StartDate,
                    Slug = createComprtition.Slug,
                    EndDate = createComprtition.EndDate,
                    Theme = createComprtition.Theme,
                    Description = createComprtition.Description,
                    UserCreate = user.Id,
                };
                _context.Competitions.Add(cp);
                _context.SaveChanges();
                return Ok(createComprtition);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult update([FromQuery] string slug, updateCompetitionDto updateCompetition)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null) return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                var cp = _context.Competitions.FirstOrDefault(c => c.Slug == slug);
                if (cp == null) return BadRequest("Competition is not Exists");
                cp.Name = updateCompetition.Name != null ? updateCompetition.Name : cp.Name;
                cp.Slug = updateCompetition.Slug != null ? updateCompetition.Slug : cp.Slug;
                cp.StartDate = updateCompetition.StartDate != null ? updateCompetition.StartDate : cp.StartDate;
                cp.EndDate = updateCompetition.EndDate != null ? updateCompetition.EndDate : cp.EndDate;
                cp.Theme = updateCompetition.Theme != null ? updateCompetition.Theme : cp.Theme;
                cp.Description = updateCompetition.Description != null ? updateCompetition.Description : cp.Description;
                cp.UserCreate = user.Id;
                cp.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return Ok(updateCompetition);
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
                var user = UserHelper.GetUserDataDto(identity);
                var cp = _context.Exibitions.Find(id);
                if (cp == null || cp.StartDate < DateTime.Now) return BadRequest("Can not delete");
                cp.Status = "Cancell";
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private void UpdateStatus(object state)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                List<Competition> competitions = _context.Competitions.Where(x => x.EndDate >= currentTime).ToList();

                foreach (var competition in competitions)
                {


                    if (currentTime < competition.StartDate)
                    {

                        competition.Status = "Coming";
                    }
                    else if (currentTime >= competition.StartDate && currentTime <= competition.EndDate)
                    {
                        competition.Status = "Process";
                    }
                    else if (currentTime > competition.EndDate)
                    {
                        competition.Status = "Finished";
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                StatusCode(500, ex.Message);
            }
        }
    }
}
