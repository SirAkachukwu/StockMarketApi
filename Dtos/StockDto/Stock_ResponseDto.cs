using StockMarketApi.Dtos.CommentDto;
using StockMarketApi.Model;
using System.ComponentModel.DataAnnotations;

namespace StockMarketApi.Dtos.StockDto
{
    public class Stock_ResponseDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(5, ErrorMessage = "Symbol cannot be more than 5 Characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage = "Company cannot be more than 10 Characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 99.99)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot be more than 10 Characters")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(1, 5000000000000)]
        public long MarketCap { get; set; }

        public List<Comment_ResponseDto>? Comments { get; set; }
    }
}
