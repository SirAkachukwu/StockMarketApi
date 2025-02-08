using Microsoft.IdentityModel.Tokens;
using StockMarketApi.Model;
using StockMarketApi.Services.Tokenservices.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockMarketApi.Services.Tokenservices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;        // Pull stuffs from appsetting.json
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }

        // putting claim into our jwt token
        public string CreateToken(ApiUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            //Create the signing credential - what type of encription do ou want
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Create the token as an Object representation of the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience =_config["JWT:Audience"]
            };

            // Create the actual token
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
