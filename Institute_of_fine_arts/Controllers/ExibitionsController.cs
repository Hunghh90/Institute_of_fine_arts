using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/exibition")]
    [ApiController]
    [Authorize(Policy = "Auth")]
    public class ExibitionsController : ControllerBase
    {
        private InstituteOfFineArtsContext _context;
        private Timer _timer;
        public ExibitionsController(InstituteOfFineArtsContext context)
        {
            _context = context;
            //_timer = new Timer(UpdateStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult get([FromQuery] string query)
        {
            try
            {
                List<exibitionDto> data;
                if (query != null)
                {
                    data = _context.Exibitions
                       .Where(data => data.Name.Contains(query))
                       .Select(u => new exibitionDto
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
                    data = _context.Exibitions
                   .Select(u => new exibitionDto
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
        public IActionResult create(createExibitionDto createExibition)
        {
            try
            {
                if (createExibition.StartDate <= DateTime.Now.AddDays(7) || createExibition.EndDate <= createExibition.StartDate)
                    return BadRequest("Invalid dates. Start date must be greater than the current time and end date must be greater than the start date.");

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var user = UserHelper.GetUserDataDto(identity);
                var check = _context.Competitions.FirstOrDefault(c => c.Name.ToLower() == createExibition.Name.ToLower());
                if (check != null) return BadRequest("Competition is Exists");
                var ex = new Entities.Exibition
                {
                    Name = createExibition.Name,
                    StartDate = createExibition.StartDate,
                    Slug = createExibition.Slug,
                    EndDate = createExibition.EndDate,
                    Theme = createExibition.Theme,
                    Description = createExibition.Description,
                    UserCreate = user.Id,
                };
                _context.Exibitions.Add(ex);
                _context.SaveChanges();
                return Ok(createExibition);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Policy = "Manager")]
        public IActionResult update([FromQuery] string slug, updateExibitionDto updateExibition)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var user = UserHelper.GetUserDataDto(identity);
                var ex = _context.Exibitions.FirstOrDefault(c => c.Slug == slug);
                if (ex == null) return BadRequest("Competition is not Exists");
                ex.Name = updateExibition.Name != null ? updateExibition.Name : ex.Name;
                ex.Slug = updateExibition.Slug != null ? updateExibition.Slug : ex.Slug;
                ex.StartDate = updateExibition.StartDate != null ? updateExibition.StartDate : ex.StartDate;
                ex.EndDate = updateExibition.EndDate != null ? updateExibition.EndDate : ex.EndDate;
                ex.Theme = updateExibition.Theme != null ? updateExibition.Theme : ex.Theme;
                ex.Description = updateExibition.Description != null ? updateExibition.Description : ex.Description;
                ex.UserCreate = user.Id;
                ex.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Policy = "Manager")]
        public IActionResult delete([FromRoute] int id)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var user = UserHelper.GetUserDataDto(identity);
                var ex = _context.Exibitions.Find(id);
                if (ex == null || ex.StartDate < DateTime.Now) return BadRequest("Can not delete");
                ex.Status = "Cancell";
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
            DateTime currentTime = DateTime.Now;
            List<Exibition> exibitions = _context.Exibitions.Where(x => x.EndDate >= currentTime).ToList();
            if (exibitions.Count < 1) return;

            foreach (var exibition in exibitions)
            {


                if (currentTime < exibition.StartDate)
                {

                    exibition.Status = "Coming";
                }
                else if (currentTime >= exibition.StartDate && currentTime <= exibition.EndDate)
                {
                    exibition.Status = "Process";
                }
                else if (currentTime > exibition.EndDate)
                {
                    exibition.Status = "Finished";
                }
            }

            _context.SaveChanges();
        }
    }
}
