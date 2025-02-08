using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.Dtos.StockDto;
using StockMarketApi.Helper;
using StockMarketApi.Mappers;
using StockMarketApi.Services.StockServices.Interfaces;

namespace StockMarketApi.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {      
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetAllAsync(queryObject);

            var stockDto = stocks.Select(s => s.ToStockResponseDto());

            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        [Authorize]
          
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepository.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockResponseDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Stock_RequestDto stockRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (stockRequestDto == null)
            {
                return BadRequest("Invalid Data");
            }

            // Check if a stock with the same symbol or company name already exists
            var existingStock = await _stockRepository.GetBySymbolOrCompanyNameAsync(stockRequestDto.Symbol, stockRequestDto.CompanyName);
            
            if (existingStock != null)
            {
                return Conflict($"A stock with the symbol '{stockRequestDto.Symbol}' or company name '{stockRequestDto.CompanyName}' already exists. Choose another name or symbol");
            }

            // Convert Stock_RequestDto to Stock
            var stock = stockRequestDto.ToStockFromStockRequestDto();

            // Save to database
            await _stockRepository.CreateAsync(stock);

            //Return the created Stock as a response
            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateDto([FromRoute] int id, [FromBody] Stock_UpdateDto stockUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (stockUpdateDto == null)
            {
                return BadRequest("Invalid Data");
            }

            var stock = await _stockRepository.UpdateAsync(id, stockUpdateDto);

            if (stock == null)
            {
                return NotFound($"Stock with the Id {id} , not found.");
            }
            
            return Ok(stock.ToStockResponseDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepository.DeleteAsync(id);
            
            if (stock == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }

}
