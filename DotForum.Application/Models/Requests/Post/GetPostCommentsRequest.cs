using DotForum.Domain.Models;

namespace DotForum.Application.Models.Requests.Post;

public class GetPostCommentsRequest : PaginationRequestModel
{
    public string PostId { get; set; }
}