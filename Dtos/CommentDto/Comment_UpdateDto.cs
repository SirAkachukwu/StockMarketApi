using System.ComponentModel.DataAnnotations;

namespace StockMarketApi.Dtos.CommentDto
{
    public class Comment_UpdateDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be minimum of 5 Character")]
        [MaxLength(280, ErrorMessage = "Title cannot be more than 280 Characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be minimum of 5 Character")]
        [MaxLength(280, ErrorMessage = "Content cannot be more than 280 Characters")]
        public string Content { get; set; } = string.Empty;
    }
}
