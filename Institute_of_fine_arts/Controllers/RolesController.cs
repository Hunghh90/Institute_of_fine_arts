using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        public RolesController(InstituteOfFineArtsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult create(createRoleDto createRole)
        {
            try
            {
                var role = _context.Roles.FirstOrDefault(r => r.Name.ToLower() == createRole.Name.ToLower());
                if (role != null) return BadRequest("Role is Exist");
                var r = new Entities.Role
                {
                    Name = createRole.Name,
                };
                _context.Roles.Add(r);
                _context.SaveChanges();
                return Ok(createRole);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            };
        }

        [HttpGet]
        public IActionResult get()
        {
            try
            {
                var data = _context.Roles.Select(r => new roleDto { Name = r.Name }).ToList();
                if (data == null) return BadRequest("No role has been initialized yet");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult update([FromRoute] int id, updateRoleDto updateRole)
        {
            try
            {
                var data = _context.Roles.Find(id);
                if (data == null) return BadRequest("Role has been initialized yet");
                data.Name = updateRole.Name;
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            };
        }

        [HttpDelete]
        public IActionResult delete([FromRoute] int id)
        {
            try
            {
                var role = _context.Roles.Find(id);
                if (role == null) return NotFound("Not found role");
                _context.Roles.Remove(role);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
