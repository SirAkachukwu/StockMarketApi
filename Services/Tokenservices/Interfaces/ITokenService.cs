using StockMarketApi.Model;

namespace StockMarketApi.Services.Tokenservices.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApiUser user);
    }
}
