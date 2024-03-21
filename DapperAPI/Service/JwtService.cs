using DapperAPI.dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API
{
    public class JwtService
    {
        private readonly string key = string.Empty;
        private readonly int duration;

        public JwtService(IConfiguration configuration)
        {
            key = configuration["Jwt:Key"]!;
            duration = int.Parse(configuration["Jwt:Duration"]!);
        }

        public string GenerateToken(userLoginResponse user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.key));
            var signingKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("id", user.userid.ToString()),
            };
            var jwtToken = new JwtSecurityToken(
                issuer: "api",
                audience: "api",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(this.duration),
                signingKey);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
