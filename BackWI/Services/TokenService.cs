using System.IdentityModel.Tokens.Jwt;
using BackWI.Data;
using Microsoft.AspNetCore.Http;
namespace BackWI.Services
{
    public class TokenService : ITokenService
    {
        public Guid GetContentByToken(HttpContext context ,string variableId)
        {
            // Obtener el token desde el usuario
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            string? jwtToken = authorizationHeader?.Split(' ').LastOrDefault();

            // Extraer el "id" desde el Claim del JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
            string? userId = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == variableId)?.Value;

            // Reconvertirlo a un Guid
            var ownerId = Guid.Parse(userId);
            return ownerId;

        }
    }
}
