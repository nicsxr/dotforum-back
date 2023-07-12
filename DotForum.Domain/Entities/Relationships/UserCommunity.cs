namespace DotForum.Domain.Entities.Relationships;

public class UserCommunity
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public string CommunityId { get; set; }
    public Community Community { get; set; }
}