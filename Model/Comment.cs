using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarketApi.Model
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }
        public Stock? Stock { get; set; } //Navigation Propety

        // Implementing one to one relationship - Connecting a comment explixitly to a user
        public string ApiUserId { get; set; }
        public ApiUser ApiUser { get; set; } // Navigation property to the apiUser
    }
}
 