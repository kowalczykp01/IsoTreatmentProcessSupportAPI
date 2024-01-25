using IsoTreatmentProcessSupportAPI.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public interface ITokenService
    {
        int GetUserIdFromToken(string token);
    }
    public class TokenService : ITokenService
    {
        public int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (decodedToken != null)
            {
                var userIdClaim = decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    return Int32.Parse(userIdClaim.Value);
                }
            }
            throw new NotFoundException("User not found");
        }
    }
}
