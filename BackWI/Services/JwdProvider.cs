using BackWI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackWI.Services
{
    public sealed class JwtProvider : IJwtProvider
        {
            private readonly IConfiguration configuration;
            public JwtProvider(IConfiguration configuration)
            {
                this.configuration = configuration;
            }
            public string CreateToken(Users user)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Nick),
                    new Claim("roleId", user.Roll),
                    new Claim("idUser", user.IdUser.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                this.configuration.GetSection("AppSettings:Token").Value!));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: creds
                );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }



        }
}