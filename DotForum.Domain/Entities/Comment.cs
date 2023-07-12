using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotForum.Domain.Entities.Relationships;

namespace DotForum.Domain.Entities;

public class Comment
{
    public string CommentId { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public string PostId { get; set; }
    public Post Post { get; set; }
    
    public string? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; }

    public ICollection<Comment> ChildComments { get; set; }
    
    public ICollection<UserCommentReaction> Reactions { get; set; } = new List<UserCommentReaction>();
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataType(DataType.DateTime)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}