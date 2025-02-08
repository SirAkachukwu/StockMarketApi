using StockMarketApi.Dtos.CommentDto;
using StockMarketApi.Model;

namespace StockMarketApi.Mappers
{
    public static class CommentMappers
    {
        public static Comment_ResponseDto ToCommentResponseDto(this Comment comment)
        {
            return new Comment_ResponseDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                CreatedBy = comment.ApiUser.UserName,
                StockId = comment.StockId
            };
        }

        public static Comment ToCommentFromCommentRequestDto(this Comment_RequestDto comment_ReqestDto, int stockId)
        {
            return new Comment
            {
                Title = comment_ReqestDto.Title,
                Content = comment_ReqestDto.Content,
                StockId = stockId
            };
        }

        public static Comment ToCommentFromCommentUpdateDto(this Comment_UpdateDto commentUpdatedto)
        {
            return new Comment
            {
                Title = commentUpdatedto.Title,
                Content = commentUpdatedto.Content
            };  
        }


    }
}
