using StockMarketApi.Dtos.StockDto;
using StockMarketApi.Helper;
using StockMarketApi.Model;

namespace StockMarketApi.Services.StockServices.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock?> GetBySymbolOrCompanyNameAsync(string symbol, string companyName);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock> UpdateAsync(int id, Stock_UpdateDto stockDto);
        Task<Stock> DeleteAsync(int id);
        Task<bool> StockExists(int id);


    }
}
