using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiWuthHush
{
    public class Tokens
    {
        public static string GenerateTokens(List<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-a-very-strong-secret-key-32-characters")); // this is security key
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // this is singing base HmacSha256

            //var claims = new[]
            //{
            //       new Claim(ClaimTypes.Name, username),
            //       new Claim(ClaimTypes.Role, "Admin") // אפשר להוסיף תפקידים או מידע נוסף
            //};
            var token = new JwtSecurityToken(
               issuer: "https://localhost:7043", //source
               audience: "https://localhost:7043",  //destanation
               claims: claims,
               expires: DateTime.Now.AddMinutes(30),  // exp date
               signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
