using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Dtos.CommentDto;
using StockMarketApi.Extensions;
using StockMarketApi.Mappers;
using StockMarketApi.Model;
using StockMarketApi.Services.CommentServices.Interfaces;
using StockMarketApi.Services.StockServices.Interfaces;

namespace StockMarketApi.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<ApiUser> _userManager;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<ApiUser> userManager)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepository.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentResponseDto());
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentResponseDto());
        }

        [HttpPost("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] Comment_RequestDto commentRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Check if stockId Exists
            if (!await _stockRepository.StockExists(stockId))
            {
                return BadRequest($"Stock with the Id {stockId} does not exist");
            }

            // Get our Username within our claims extension
            var username = User.GetUsername();
            // Get our user from database
            var apiUser = await _userManager.FindByNameAsync(username);

            // Convert CommentRequestDto to Comments
            var comment = commentRequestDto.ToCommentFromCommentRequestDto(stockId);

            // Before it is Saved to the database, we'll pass in our appUserid to identify who is making the comment (one to one relation)
            comment.ApiUserId = apiUser.Id;

            // Save in Database
            await _commentRepository.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentResponseDto());
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Comment_UpdateDto commentUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (commentUpdateDto == null)
            {
                return BadRequest("Invalid Data");
            }
            
            //Call the repository to update the comment
            var comment = await _commentRepository.UpdateAsync(id, commentUpdateDto.ToCommentFromCommentUpdateDto());

            // Handle the case where no comment was found
            if (comment == null)
            {
                return NotFound($"Comment with the Id '{id}' not found");
            }
            return Ok(comment.ToCommentResponseDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepository.DeleteAsync(id);
            if (comment == null)
            {
                return NotFound($"Comment with the id '{id}' does not exist");
            }
            return Ok("Deleted");
        }

    }
}
