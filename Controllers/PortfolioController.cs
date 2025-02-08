using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Model;
using StockMarketApi.Extensions;
using StockMarketApi.Services.StockServices.Interfaces;
using StockMarketApi.Services.PortfolioServices.Interface;

namespace StockMarketApi.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(UserManager<ApiUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetUserPortfolio()
        {
            // Retreives autrhenticated user's username using claim method
            var username = User.GetUsername(); //User(inherited from controllerbase) allows us to reach out to claims and use its fuction within the http context

            // Find the user by username
            var apiUser = await _userManager.FindByNameAsync(username);

            // Find the user portfolio
            var userPortfolio  = await _portfolioRepository.GetUserPortfolio(apiUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> CreateUserPortfolio(string symbol)
        {
            // Retrieve the Authenticated User
            var username = User.GetUsername();
            var apiUser = await _userManager.FindByNameAsync(username);

            // Check if the stock Exists by symbol
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if(stock == null)
            {
                return BadRequest("Stock not found");
            }

            // Check for Duplicate Stocks
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(apiUser);
            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Cannot add same stock to portfolio");
            }

            // Creates new portfolio
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                ApiUserId = apiUser.Id
            };

            await _portfolioRepository.CreatePortfolioAsync(portfolioModel);

            if(portfolioModel ==  null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Ok("Successfully Created");
            }

        }

        [HttpDelete]
        [Authorize]

        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var apiUser = await _userManager.FindByNameAsync(username);

            //if(apiUser == null)
            //{
            //    return BadRequest("Not found");
            //}  // Not really necessary

            //get all the stock in the user portfolio
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(apiUser);

            // Check if the Stock Exists in the Portfolio
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());

            if(filteredStock.Count() == 1)
            {
                await _portfolioRepository.DeletePortfolio(apiUser, symbol);
            }
            else
            {
                return BadRequest("Stock is not in your portfolio");
            }

            return Ok("Deleted Successfully");
        }

    }
}
