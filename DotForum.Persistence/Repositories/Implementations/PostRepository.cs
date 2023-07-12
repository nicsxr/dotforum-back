using DotForum.Domain.Entities;
using DotForum.Domain.Enums;
using DotForum.Domain.Models;
using DotForum.Persistence.DbContexts;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DotForum.Persistence.Repositories.Implementations;

public class PostRepository : CommonRepository<Post>, IPostRepository
{
    public PostRepository(DotForumDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Post>> GetHomePublicPosts(int pageSize, int page)
    {
        var itemsToSkip = (page - 1) * pageSize;

        var posts = await DbContext.Posts.OrderByDescending(p => p.Timestamp).Skip(itemsToSkip).Take(pageSize)
         .ToListAsync();
        return posts;
    }

    public async Task<PostModel> GetPostModelById(string id, string? userId = null)
    {
        var postModel = await DbContext.Posts
            .Where(p => p.Id == id)
            .Select(p => new PostModel
            {
                Id = p.Id,
                CommunityId = p.CommunityId,
                CommunityName = p.Community.Name,
                UserId = p.UserId,
                Username = p.User.UserName!,
                Title = p.Title,
                Body = p.Body,
                TotalComments = p.Comments.Count,
                Upvotes = p.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                Downvotes = p.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                Vote = userId != null ? p.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote
            }).FirstOrDefaultAsync();
        
        return postModel;
    }

    public IQueryable<Post> GetQueryableById(string postId)
    {
        return DbContext.Posts.Where(p => p.Id == postId);
    }

    public IQueryable<Post> GetQueryableAll()
    {
        return DbContext.Posts;
    }
}