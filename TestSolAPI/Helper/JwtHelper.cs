using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TestSolAPI.Models;

namespace TestSolAPI.Helper
{
    public static class JwtHelper
    {
        public static User GenerateToken(User user, JwtSettings jwtSettings)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                // Obtain SECRET KEY
                byte[] key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);

                Guid Id;

                // Expires in 1 day
                DateTime expiredTime = DateTime.UtcNow.AddDays(1);

                // Validity of our token
                user.Validity = expiredTime.TimeOfDay;

                // Generate our JWT
                JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer,
                    audience: jwtSettings.ValidAudience,
                    claims: GetClaims(user, out Id),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(expiredTime).DateTime,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

                return new User()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                    Username = user.Username,
                    Id = user.Id,
                    GuidId = Id
                };
            }
            catch (System.Exception e)
            {
                throw new Exception($"Error generating the JWT: {e.Message}");
            }
        }

        private static IEnumerable<Claim> GetClaims(this User user, out Guid Id)
        {
            Id = Guid.NewGuid();
            return GetClaims(user, Id);
        }
        private static IEnumerable<Claim> GetClaims(this User user, Guid Id)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.EmailId),
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MM ddd dd yyyy HH:mm:ss tt"))
            };

            if (user.Username.Equals("Admin"))
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            else
                claims.Add(new Claim(ClaimTypes.Role, "User"));

            return claims;
        }
    }
}