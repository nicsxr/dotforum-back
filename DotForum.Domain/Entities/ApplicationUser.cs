using DotForum.Domain.Entities.Relationships;

namespace DotForum.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public ICollection<Post>? Posts { get; set; }
    public ICollection<UserCommunity>? FollowedCommunities { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public ICollection<UserPostReaction> PostReactions { get; set; } = new List<UserPostReaction>();
    
    public ICollection<UserCommentReaction> CommentReactions { get; set; } = new List<UserCommentReaction>();
}