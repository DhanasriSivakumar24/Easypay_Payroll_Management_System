using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Easypay_App.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration) 
        { 
            string secret = configuration["Tokens:JWT"]??" ".ToString();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
        public async Task<string> GenerateToken(LoginResponseDTO login)
        {
            //Payload for the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.UserName),
                new Claim(ClaimTypes.Role, login.Role)
            };

            //algo used for tokken
            var cred = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256Signature);

            //loading token details
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = cred
            };

            //generating the token
            var tokenHandlers = new JwtSecurityTokenHandler();
            var token = tokenHandlers.CreateToken(descriptor);
            return tokenHandlers.WriteToken(token);
        }
    }
}
