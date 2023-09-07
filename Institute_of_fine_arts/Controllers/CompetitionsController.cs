using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/competition")]
    [ApiController]
    [Authorize(Policy = "Auth")]
    public class CompetitionsController : ControllerBase
    {
        private InstituteOfFineArtsContext _context;
        private Timer _timer;
        public CompetitionsController(InstituteOfFineArtsContext context)
        {
            _context = context;

        }
        //public void StartTimer()
        //{
        //    _timer = new Timer(UpdateStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        //}

        [HttpPost]
        [Route("upload")]
        [Authorize(Policy = "Manager")]
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
                var path = "wwwroot/uploads/competitions";
                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(image.FileName);
                var upload = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
                image.CopyTo(new FileStream(upload, FileMode.Create));
                var rs = $"{Request.Scheme}://{Request.Host}/uploads/competitions/{fileName}";
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
        [AllowAnonymous]
        public IActionResult GetCompetitions([FromQuery] getArtsDto dto)
        {
            if (dto.Page == null) dto.Page = 1;
            if (dto.Limit == null) dto.Limit = 30;
            int startIndex = (dto.Page.Value - 1) * dto.Limit.Value;
            int endIndex = dto.Page.Value * dto.Limit.Value;
            if(!string.IsNullOrEmpty(dto.Search)&& dto.Search == "Status:Granded")
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                if (user == null) return Unauthorized();
                var competitions = _context.Competitions
                    .Where(x=> x.Status =="Granded")
                    .Where(x => x.TeacherId1 == user.Id || x.TeacherId2 == user.Id || x.TeacherId3 == user.Id || x.TeacherId4 == user.Id || x.TeacherId5 == user.Id)
                    .ToList();

                List<Competition> resultss = competitions.Skip(startIndex).Take(dto.Limit.Value).ToList();
                string urls = $"/competitions?search={dto.Search}&limit={dto.Limit}";

                var data = new Dictionary<string, object>
                {
                    { "data", resultss }
                };

                var paginations = PaginationHelper.paginate(competitions.Count(), dto.Page.Value, dto.Limit.Value, resultss.Count, urls);
                foreach (var property in paginations.GetType().GetProperties())
                {
                    data.Add(property.Name, property.GetValue(paginations));
                }

                return Ok(data);
            }
            List<Competition> query = _context.Competitions
                .Include(x=>x.Prizes)
                .Include(x => x.TeacherId1Navigation)
                .Include(x => x.TeacherId2Navigation)
                .Include(x => x.TeacherId3Navigation)
                .Include(x => x.TeacherId4Navigation)
                .Include(x => x.TeacherId5Navigation)
                .Include(x => x.Arts).ToList();

            if (!string.IsNullOrEmpty(dto.Search))
            {
                string[] parseSearchParams = dto.Search.Split(';');
                List<Dictionary<string, string>> searchText = new List<Dictionary<string, string>>();

                foreach (string searchParam in parseSearchParams)
                {
                    string[] keyValue = searchParam.Split(':');
                    string key = keyValue[0];
                    string value = keyValue[1];

                    searchText.Add(new Dictionary<string, string>
                    {
                        { key, value }
                    });
                }


                query = query.Where(competition =>
                    searchText.All(search =>
                         competition.GetType().GetProperty(search.Keys.First())?.GetValue(competition)?.ToString() == search.Values.First()
                    )
                ).ToList();
            }
            List<Competition> results = query.Skip(startIndex).Take(dto.Limit.Value).ToList();
            string url = $"/competitions?search={dto.Search}&limit={dto.Limit}";

            var result = new Dictionary<string, object>
            {
                { "data", results }
            };

            var pagination = PaginationHelper.paginate(query.Count(), dto.Page.Value, dto.Limit.Value, results.Count, url);
            foreach (var property in pagination.GetType().GetProperties())
            {
                result.Add(property.Name, property.GetValue(pagination));
            }

            return Ok(result);
        }
        [HttpGet("{slug}")]
        [AllowAnonymous]
        public IActionResult getSlug([FromRoute] string? slug)
        {
            try
            {
                var data = _context.Competitions
                       .Where(data => data.Slug == slug)
                       .Select(u => new competitionDto
                       {
                           Id = u.Id,
                           Name = u.Name,
                           StartDate = u.StartDate,
                           Slug = u.Slug,
                           EndDate = u.EndDate,
                           Description = u.Description,
                           Theme = u.Theme,
                           Image = u.Image,
                       })
                       .FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("top")]
        public IActionResult topCompetition()
        {
            try
            {
                var data = _context.Competitions.OrderByDescending(c => c.Prizes.Sum(p => p.Price * p.Quantity)).ToList();
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
                    StartDate = createComprtition.StartDate.Value,
                    Slug = createComprtition.Slug,
                    EndDate = createComprtition.EndDate.Value,
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
        [Authorize(Policy = "Manager")]
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
                cp.StartDate = updateCompetition.StartDate != null ? updateCompetition.StartDate.Value : cp.StartDate;
                cp.EndDate = updateCompetition.EndDate != null ? updateCompetition.EndDate.Value : cp.EndDate;
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
        [Authorize(Policy = "Manager")]
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
                var competitions = _context.Competitions.Where(x => x.EndDate >= currentTime).ToList();

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
