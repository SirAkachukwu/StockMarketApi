using System.ComponentModel.DataAnnotations;

namespace StockMarketApi.Dtos.AccountDto
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
