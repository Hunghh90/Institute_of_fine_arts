using Institute_of_fine_arts.Dto;
using System.Security.Claims;

namespace Institute_of_fine_arts.Helpers
{
    public static class UserHelper
    {
        public static userDataDto GetUserDataDto(ClaimsIdentity identity)
        {
            if (identity == null) throw new ArgumentNullException();
            var userClaims = identity.Claims;
            var Id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            return new userDataDto
            {
                Id = Convert.ToInt32(Id),
                Name = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                RoleId = Convert.ToInt32(Role),
            };
        }
    }
}
