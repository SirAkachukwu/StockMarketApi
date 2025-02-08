using Microsoft.AspNetCore.Identity;

namespace StockMarketApi.Model
{
    public class ApiUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

    }
}
