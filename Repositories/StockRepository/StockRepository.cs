using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.Dtos.StockDto;
using StockMarketApi.Helper;
using StockMarketApi.Model;
using StockMarketApi.Services.StockServices.Interfaces;

namespace StockMarketApi.Repositories.StockRepository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock> DeleteAsync(int id)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stock == null)
            {
                return null;
            }            
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        //// GET ALL METHOD BEFORE QUERY IS INVOLVED
        //public async Task<List<Stock>> GetAllAsync()
        //{
        //    return await _context.Stock.Include(c => c.Comments).ToListAsync();
        //}

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stock.Include(c => c.Comments).ThenInclude(a => a.ApiUser).AsQueryable();   // ThenInclude is LINQ querry that will make sure we also include our app user

            // Filtering - (AsQuertable gives us that power)

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            // Sorting and Ordering

            if (!string.IsNullOrWhiteSpace(query.SortBy))

                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }

            // Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }


        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stock = await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
            if (stock == null)
            {
                return null;
            }
            return stock;
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<Stock?> GetBySymbolOrCompanyNameAsync(string symbol, string companyName)
        {
            return await _context.Stock.FirstOrDefaultAsync(s => s.Symbol.ToLower() == symbol.ToLower() || s.CompanyName.ToLower() == companyName.ToLower());
        }

        public async Task<bool> StockExists(int id)
        {
            var stockExists = await _context.Stock.AnyAsync(x => x.Id == id);
            if (stockExists == false)
            {
                return false;
            }
            return stockExists;
        }

        public async Task<Stock> UpdateAsync(int id, Stock_UpdateDto stockUpdateDto)
        {
            var existingStock = await _context.Stock.FirstOrDefaultAsync(x =>x.Id == id);
            if (existingStock == null)
            {
                return null;
            }
            //If you find the id you want to update, map the update dto properties to the existing entity
            existingStock.Symbol = stockUpdateDto.Symbol;
            existingStock.CompanyName = stockUpdateDto.CompanyName;
            existingStock.Purchase = stockUpdateDto.Purchase;
            existingStock.LastDiv = stockUpdateDto.LastDiv;
            existingStock.Industry = stockUpdateDto.Industry;
            existingStock.MarketCap = stockUpdateDto.MarketCap;

            //save the updatd entity to the db
            await _context.SaveChangesAsync();
            return (existingStock);
        }
    } 
}
