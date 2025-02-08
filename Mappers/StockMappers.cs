using StockMarketApi.Dtos.StockDto;
using StockMarketApi.Model;

namespace StockMarketApi.Mappers
{
    public static class StockMappers
    {
        public static Stock_ResponseDto ToStockResponseDto(this Stock stockModel)
        {
            return new Stock_ResponseDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentResponseDto()).ToList()
            };
        }

        public static Stock ToStockFromStockRequestDto(this Stock_RequestDto stock_RequestDto)
        {
            return new Stock
            {
                Symbol = stock_RequestDto.Symbol,
                CompanyName = stock_RequestDto.CompanyName,
                Purchase = stock_RequestDto.Purchase,
                LastDiv = stock_RequestDto.LastDiv,
                Industry = stock_RequestDto.Industry,
                MarketCap = stock_RequestDto.MarketCap
            };
        }

    }
}
