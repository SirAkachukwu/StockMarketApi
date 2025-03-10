﻿using System.ComponentModel.DataAnnotations;

namespace StockMarketApi.Dtos.StockDto
{
    public class Stock_RequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot be more than 0 Characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(20, ErrorMessage = "Company cannot be more than 20 Characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 99.99)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "Industry cannot be more than 15 Characters")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(1, 50000000000000)]
        public long MarketCap { get; set; }

    }
}
