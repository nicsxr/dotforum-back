using DotForum.Domain.Entities;
using DotForum.Domain.Enums;

namespace DotForum.Domain.Models;

public class GetCommentsModel
{
    public string CommentId { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; }

    public string UserId { get; set; }
    public string Username { get; set; }

    public string PostId { get; set; }

    public string? ParentCommentId { get; set; }
    
    public int ChildCommentsCount { get; set; }
    
    public int Upvotes { get; set; }
    
    public int Downvotes { get; set; }
    
    public VoteStatusEnum Vote { get; set; }
    
    public ICollection<GetCommentsModel> ChildComments { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}