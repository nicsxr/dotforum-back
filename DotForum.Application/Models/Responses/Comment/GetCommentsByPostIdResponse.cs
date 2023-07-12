namespace DotForum.Application.Models.Responses.Comment;

public class GetCommentsByPostIdResponse
{
    public string CommentId { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; }

    public string UserId { get; set; }
    public string Username { get; set; }

    public string PostId { get; set; }

    public string? ParentCommentId { get; set; }
    
    public int ChildCommentsCount { get; set; }
    
    public ICollection<GetCommentsByPostIdResponse> ChildComments { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}