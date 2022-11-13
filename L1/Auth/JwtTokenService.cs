using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace L1.Auth
{
    public interface IJwtTokenService
    {
        public string CreateAccessToken(string userName, string userId, IEnumerable<string> userRoles);
        public string ReturnGuid();
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly SymmetricSecurityKey _authSigningKey;
        private readonly string _issuer;
        private readonly string _audience;
        public static string guid { get; set; }

        public JwtTokenService(IConfiguration configuration)
        {
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _issuer = configuration["JWT:ValidIssuer"];
            _audience = configuration["JWT:ValidAudience"];
        }

        public string ReturnGuid()
        {
            return guid;
        }

        public string CreateAccessToken(string userName, string userId, IEnumerable<string> userRoles)
        {
            guid = Guid.NewGuid().ToString();
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new(JwtRegisteredClaimNames.Jti, guid),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
            };

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var accessSecurityToken = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                expires: DateTime.UtcNow.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha512));

            return new JwtSecurityTokenHandler().WriteToken(accessSecurityToken);
        }
    }
}
