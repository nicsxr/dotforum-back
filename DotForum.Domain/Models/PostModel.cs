using DotForum.Domain.Enums;

namespace DotForum.Domain.Models;

public class PostModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string CommunityId { get; set; }
    
    public string CommunityName { get; set; }
    
    public string UserId { get; set; }
    
    public string Username { get; set; }
    
    public string Title { get; set; }
    
    public string Body { get; set; }

    public int TotalComments { get; set; }
    
    public int Upvotes { get; set; }
    
    public VoteStatusEnum Vote { get; set; }
    
    public int Downvotes { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}