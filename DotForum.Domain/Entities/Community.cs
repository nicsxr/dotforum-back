using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotForum.Domain.Entities.Relationships;

namespace DotForum.Domain.Entities;

public class Community
{
    public string Id { get; set; }
    public ApplicationUser User { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public ICollection<Post>? Posts { get; set; }
    
    public ICollection<UserCommunity>? Followers { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataType(DataType.DateTime)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}