﻿using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/prize")]
    [ApiController]
    public class PrizesController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        public PrizesController(InstituteOfFineArtsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult create(createPrizeDto createPrize)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var user = UserHelper.GetUserDataDto(identity);
                var prize = _context.Prizes
                    .FirstOrDefault(u =>
                    u.Name.ToLower() == createPrize.Name.ToLower() &&
                    u.ConpetitionId == createPrize.CompetitionId);
                if (prize != null) return BadRequest("Prize is Exist");
                var pr = new Entities.Prize
                {
                    Name = createPrize.Name,
                    Detail = createPrize.Detail,
                    Quantity = createPrize.Quantity,
                    ConpetitionId = createPrize.CompetitionId,
                    UserCreate = user.Id,
                };
                _context.Prizes.Add(pr);
                _context.SaveChanges();
                return Ok(createPrize);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        public IActionResult getByCompetition([FromQuery] int id)
        {
            try
            {
                var data = _context.Prizes
                    .Where(p => p.ConpetitionId == id)
                    .Select(s => new prizeDto
                    {
                        Name = s.Name,
                        Detail = s.Detail,
                        Quantity = s.Quantity
                    })
                    .ToList();
                if (data.Count < 1) return BadRequest("There are no prizes for this contest yet");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult update([FromRoute] int id, updatePrizeDto updatePrize)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null) return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                var p = _context.Prizes.Find(id);
                if (p == null) return NotFound("Not found prize");
                var cp = _context.Competitions.Find(p.ConpetitionId);
                if (cp == null || cp.StartDate >= DateTime.Now) return BadRequest("Can't be changed");
                p.Name = updatePrize.Name != null ? updatePrize.Name : p.Name;
                p.Detail = updatePrize.Detail != null ? updatePrize.Detail : p.Detail;
                p.Quantity = updatePrize.Quantity != null ? updatePrize.Quantity : p.Quantity;
                p.UserCreate = user.Id;
                _context.SaveChanges();
                return NoContent();
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
                var p = _context.Prizes.Find(id);
                if (p == null) return NotFound("Not found prize");
                var cp = _context.Competitions.Find(p.ConpetitionId);
                if (cp == null || cp.StartDate >= DateTime.Now) return BadRequest("Can't be delete");
                p.Status = "Cancell";
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
