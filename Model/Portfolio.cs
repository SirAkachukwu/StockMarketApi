using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarketApi.Model
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string ApiUserId { get; set; }
        public int StockId { get; set; }
        public ApiUser ApiUser { get; set; }
        public Stock Stock { get; set; }
    }
}
