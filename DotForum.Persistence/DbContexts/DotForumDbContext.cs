using DotForum.Domain.Entities;
using DotForum.Domain.Entities.Relationships;
using DotForum.Persistence.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Persistence.DbContexts;

public class DotForumDbContext : DbContext
{
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Community> Communities { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserCommunity> UserCommunities { get; set; }
    public DbSet<UserPostReaction> UserPostReactions { get; set; }
    public DbSet<UserCommentReaction> UserCommentReactions { get; set; }

    public DotForumDbContext(DbContextOptions<DotForumDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //
        // builder.Entity<Post>().Property(e => e.Id)
        //     .HasAnnotation("SqlServer:Identity", "200, 1");
        builder.Entity<Post>().Property(c => c.Id).HasMaxLength(36)
            .ValueGeneratedOnAdd();        
        builder.Entity<Post>().Property(c => c.MediaUrl)
            .IsRequired(false);
        builder.Entity<Community>().Property(c => c.Id).HasMaxLength(36)
            .ValueGeneratedOnAdd();

        builder.Entity<Community>().HasIndex(c => c.Name).IsUnique();
        builder.Entity<Community>().HasIndex(c => c.NormalizedName).IsUnique();

        builder.Entity<ApplicationUser>()
            .Property(u => u.Email)
            .IsRequired(false);
        
        
        builder.Entity<UserCommunity>()
            .HasKey(cf => new { cf.UserId, cf.CommunityId });


        builder.Entity<UserCommunity>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.FollowedCommunities)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserCommunity>()
            .HasOne(uc => uc.Community)
            .WithMany(c => c.Followers)
            .HasForeignKey(uc => uc.CommunityId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Community>()
            .HasMany(c => c.Posts)
            .WithOne(p => p.Community)
            .HasForeignKey(p => p.CommunityId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.Entity<Comment>()
            .HasKey(c => c.CommentId);

        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.ChildComments)
            .HasForeignKey(c => c.ParentCommentId)
            .IsRequired(false);
        
        
        builder.Entity<UserPostReaction>()
            .HasKey(upr => upr.UserPostReactionId);

        builder.Entity<UserPostReaction>()
            .Property(upr => upr.VoteStatus)
            .HasConversion<int>();
        
        builder.Entity<UserPostReaction>()
            .HasOne(upr => upr.User)
            .WithMany(u => u.PostReactions)
            .HasForeignKey(upr => upr.UserId);

        builder.Entity<UserPostReaction>()
            .HasOne(upr => upr.Post)
            .WithMany(p => p.Reactions)
            .HasForeignKey(upr => upr.PostId);        
        
        builder.Entity<UserCommentReaction>()
            .HasKey(ucr => ucr.UserCommentReactionId);

        builder.Entity<UserCommentReaction>()
            .Property(ucr => ucr.VoteStatus)
            .HasConversion<int>();
        
        builder.Entity<UserCommentReaction>()
            .HasOne(ucr => ucr.User)
            .WithMany(u => u.CommentReactions)
            .HasForeignKey(ucr => ucr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserCommentReaction>()
            .HasOne(ucr => ucr.Comment)
            .WithMany(c => c.Reactions)
            .HasForeignKey(ucr => ucr.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}