using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotForum.Domain.Entities.Relationships;

namespace DotForum.Domain.Entities;

public class Post
{
    public string Id { get; set; }
    public Community Community { get; set; }
    public string CommunityId { get; set; }
    public ApplicationUser User { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    
    public string MediaUrl { get; set; }
    
    public ICollection<Comment> Comments { get; set; }

    public ICollection<UserPostReaction> Reactions { get; set; } = new List<UserPostReaction>();

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataType(DataType.DateTime)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}