using DotForum.Domain.Models;

namespace DotForum.Application.Models.Requests.Comment;

public class GetChildCommentsRequest : PaginationRequestModel
{
    public string CommentId { get; set; }
}