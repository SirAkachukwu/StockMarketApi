using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMarketApi.Dtos.AccountDto;
using StockMarketApi.Model;
using StockMarketApi.Services.Tokenservices.Interfaces;

namespace StockMarketApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApiUser> _signInManager;

        public AccountController(UserManager<ApiUser> userManager, ITokenService tokenService, SignInManager<ApiUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            // Find the user by username
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null)
                return Unauthorized("Invalid username!");
            
            // Find the password using the signinManager
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("UserName not found and/or password incorrect");

            return Ok(
                new RegisterResponseDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Check ModelState - Validates the incoming data using the RegisterDto class.
                if (!ModelState.IsValid) 
                    return BadRequest(ModelState);

                // Create a new apiuser
                var apiUser = new ApiUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                // Attempts to create the user in the database using the provided password.
                var createdUser = await _userManager.CreateAsync(apiUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    // Assigns the "User" role to the newly created user via
                    var roleResult = await _userManager.AddToRoleAsync(apiUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new RegisterResponseDto
                            {
                                Username = apiUser.UserName,
                                Email = apiUser.Email,
                                Token = _tokenService.CreateToken(apiUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            } 
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
