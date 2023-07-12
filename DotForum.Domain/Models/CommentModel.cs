namespace DotForum.Domain.Models;

public class CommentModel
{
    public string CommentId { get; set; } = Guid.NewGuid().ToString();
    
    public string Text { get; set; }

    public string UserId { get; set; }

    public string PostId { get; set; }

    public string? ParentCommentId { get; set; }

    public ICollection<CommentModel> ChildComments { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}