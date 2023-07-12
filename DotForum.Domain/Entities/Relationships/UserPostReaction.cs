using DotForum.Domain.Enums;

namespace DotForum.Domain.Entities.Relationships;

public class UserPostReaction
{
    public string UserPostReactionId { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string PostId { get; set; }
    public Post Post { get; set; }
    
    public VoteStatusEnum? VoteStatus { get; set; }
}