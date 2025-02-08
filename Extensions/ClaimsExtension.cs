using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StockMarketApi.Extensions
{
    public static class ClaimsExtension
    {
        // A method to be able to reach into the claim - We get this claim from our token service
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var claim = user.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.GivenName);
            return claim?.Value ?? throw new NullReferenceException("GivenName claim is missing.");
        }
    }
}