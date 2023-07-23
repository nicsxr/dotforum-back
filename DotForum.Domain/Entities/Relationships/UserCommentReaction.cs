using DotForum.Domain.Enums;

namespace DotForum.Domain.Entities.Relationships;

public class UserCommentReaction
{
    public string UserCommentReactionId { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string CommentId { get; set; }
    public Comment Comment { get; set; }
    
    public VoteStatusEnum? VoteStatus { get; set; }
}