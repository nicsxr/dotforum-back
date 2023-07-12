using DotForum.Domain.Entities;
using DotForum.Domain.Models;
using DotForum.Persistence.DbContexts;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Persistence.Repositories.Implementations;

public class CommentRepository : CommonRepository<Comment>, ICommentRepository
{
    public CommentRepository(DotForumDbContext dbContext) : base(dbContext)
    {
    }

    
    // BUG: user not included
    public async Task<List<Comment>> GetCommentsByPostId(string postId, int page, int pageSize)
    {
        var itemsToSkip = (page - 1) * pageSize;

        var comments = await DbContext.Comments.Where(c => c.PostId == postId && c.ParentCommentId == null)
            .OrderByDescending(c => c.Timestamp)
            .Skip(itemsToSkip).Take(pageSize)
            .Include(c => c.ChildComments.OrderByDescending(cc => cc.Timestamp).Take(10))
            .ThenInclude(c => c.ChildComments.OrderByDescending(cc => cc.Timestamp).Take(3))
            .Include(c => c.User)
            .ToListAsync();

        return comments;
    }

    public async Task<int> GetChildCommentsCount(string commentId)
    {
        return await DbContext.Comments.CountAsync(c => c.ParentCommentId == commentId);
    }
    
    public async Task<Comment> GetByIdAsync(string commentId)
    {
        var comment =  await DbContext.Comments.Where(c => c.CommentId == commentId)
            .OrderByDescending(c => c.Timestamp)
            .Include(c => c.ChildComments.Take(5))
            .ThenInclude(cc => cc.ChildComments.Take(3))
            .Include(c => c.User)
            .FirstOrDefaultAsync();

        return comment!;
    }

    public async Task<List<Comment>> GetChildComments(string commentId, int page, int pageSize)
    {
        var itemsToSkip = (page - 1) * pageSize;

        var comments =  await DbContext.Comments.Where(c => c.ParentCommentId == commentId)
            .OrderByDescending(c => c.Timestamp)
            .Skip(itemsToSkip).Take(pageSize)
            // .Include(c => c.ChildComments.Take(5).sk)
            .Include(c => c.User)
            .ToListAsync();

        return comments;
    }
}