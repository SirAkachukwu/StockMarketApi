using StockMarketApi.Model;

namespace StockMarketApi.Services.PortfolioServices.Interface
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(ApiUser user);
        Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(ApiUser apiUser, string symbol);
    }
}
