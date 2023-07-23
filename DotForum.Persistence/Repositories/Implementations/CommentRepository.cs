using System.Collections.Immutable;
using DotForum.Domain.Entities;
using DotForum.Domain.Enums;
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
    public async Task<List<GetCommentsModel>> GetCommentsByPostId(string postId, int page, int pageSize, string? userId = null)
    {
        var itemsToSkip = (page - 1) * pageSize;

        // var comments = await DbContext.Comments.Where(c => c.PostId == postId && c.ParentCommentId == null)
        //     .OrderByDescending(c => c.Timestamp)
        //     .Skip(itemsToSkip).Take(pageSize)
        //     .Include(c => c.ChildComments.OrderByDescending(cc => cc.Timestamp).Take(10))
        //     .ThenInclude(c => c.ChildComments.OrderByDescending(cc => cc.Timestamp).Take(3))
        //     .Include(c => c.User)
        //     .ToListAsync();
        // return comments;


        var comments = await DbContext.Comments.Where(c => c.PostId == postId)
            .Skip(itemsToSkip).Take(pageSize)
            .Include(c => c.User)
            .Select(c => new GetCommentsModel
            {
                CommentId = c.CommentId,
                Text = c.Text,
                UserId = c.UserId,
                Username = c.User.UserName!,
                PostId = c.PostId,
                ParentCommentId = c.ParentCommentId,
                Upvotes = c.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                Downvotes = c.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                Vote = userId != null ? c.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                ChildCommentsCount = c.ChildComments.Count,
                ChildComments = c.ChildComments.Take(10).Select(cc => new GetCommentsModel
                {
                    CommentId = cc.CommentId,
                    Text = cc.Text,
                    UserId = cc.UserId,
                    Username = cc.User.UserName!,
                    PostId = cc.PostId,
                    ParentCommentId = cc.ParentCommentId,
                    Upvotes = cc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                    Downvotes = cc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                    Vote = userId != null ? cc.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                    ChildCommentsCount = cc.ChildComments.Count,
                    ChildComments = cc.ChildComments.Take(3).Select(gcc => new GetCommentsModel
                    {
                        CommentId = gcc.CommentId,
                        Text = gcc.Text,
                        UserId = gcc.UserId,
                        Username = gcc.User.UserName!,
                        PostId = gcc.PostId,
                        ParentCommentId = gcc.ParentCommentId,
                        Upvotes = gcc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                        Downvotes = gcc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                        Vote = userId != null ? gcc.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                        ChildCommentsCount = gcc.ChildComments.Count,
                        ChildComments = new List<GetCommentsModel>(),
                        Timestamp = gcc.Timestamp,
                    }).ToList(),
                    Timestamp = cc.Timestamp
                }).ToList(),
                Timestamp = c.Timestamp
            })
            .ToListAsync();

        return comments;
    }

    public async Task<int> GetChildCommentsCount(string commentId)
    {
        return await DbContext.Comments.CountAsync(c => c.ParentCommentId == commentId);
    }
    
    public async Task<GetCommentsModel> GetByIdAsync(string commentId, string? userId = null)
    {
        var comment =  await DbContext.Comments.Where(c => c.CommentId == commentId)
            .OrderByDescending(c => c.Timestamp)
            .Select(c => new GetCommentsModel
            {
                CommentId = c.CommentId,
                Text = c.Text,
                UserId = c.UserId,
                Username = c.User.UserName!,
                PostId = c.PostId,
                ParentCommentId = c.ParentCommentId,
                Upvotes = c.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                Downvotes = c.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                Vote = userId != null ? c.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                ChildCommentsCount = c.ChildComments.Count,
                ChildComments = c.ChildComments.Take(5).Select(cc => new GetCommentsModel
                {
                    CommentId = cc.CommentId,
                    Text = cc.Text,
                    UserId = cc.UserId,
                    Username = cc.User.UserName!,
                    PostId = cc.PostId,
                    ParentCommentId = cc.ParentCommentId,
                    Upvotes = cc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                    Downvotes = cc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                    Vote = userId != null ? cc.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                    ChildCommentsCount = cc.ChildComments.Count,
                    ChildComments = cc.ChildComments.Take(3).Select(gcc => new GetCommentsModel
                    {
                        CommentId = gcc.CommentId,
                        Text = gcc.Text,
                        UserId = gcc.UserId,
                        Username = gcc.User.UserName!,
                        PostId = gcc.PostId,
                        ParentCommentId = gcc.ParentCommentId,
                        Upvotes = gcc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                        Downvotes = gcc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                        Vote = userId != null ? gcc.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                        ChildCommentsCount = gcc.ChildComments.Count,
                        ChildComments = new List<GetCommentsModel>(),
                        Timestamp = gcc.Timestamp,
                    }).ToList(),
                    Timestamp = cc.Timestamp
                }).ToList(),
                Timestamp = c.Timestamp
            })
            .FirstOrDefaultAsync();

        return comment!;
    }

    public async Task<List<GetCommentsModel>> GetChildComments(string commentId, int page, int pageSize, string? userId = null)
    {
        var itemsToSkip = (page - 1) * pageSize;

        var comments =  await DbContext.Comments.Where(c => c.ParentCommentId == commentId)
            .OrderByDescending(c => c.Timestamp)
            .Skip(itemsToSkip).Take(pageSize)
            .Select(c => new GetCommentsModel
            {
                CommentId = c.CommentId,
                Text = c.Text,
                UserId = c.UserId,
                Username = c.User.UserName!,
                PostId = c.PostId,
                ParentCommentId = c.ParentCommentId,
                Upvotes = c.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                Downvotes = c.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                Vote = userId != null ? c.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                ChildCommentsCount = c.ChildComments.Count,
                ChildComments = c.ChildComments.Take(5).Select(cc => new GetCommentsModel
                {
                    CommentId = cc.CommentId,
                    Text = cc.Text,
                    UserId = cc.UserId,
                    Username = cc.User.UserName!,
                    PostId = cc.PostId,
                    ParentCommentId = cc.ParentCommentId,
                    Upvotes = cc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                    Downvotes = cc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                    Vote = userId != null ? cc.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                    ChildCommentsCount = cc.ChildComments.Count,
                    ChildComments = cc.ChildComments.Take(2).Select(gcc => new GetCommentsModel
                    {
                        CommentId = gcc.CommentId,
                        Text = gcc.Text,
                        UserId = gcc.UserId,
                        Username = gcc.User.UserName!,
                        PostId = gcc.PostId,
                        ParentCommentId = gcc.ParentCommentId,
                        Upvotes = gcc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                        Downvotes = gcc.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                        Vote = userId != null ? gcc.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote : VoteStatusEnum.Novote,
                        ChildCommentsCount = gcc.ChildComments.Count,
                        ChildComments = new List<GetCommentsModel>(),
                        Timestamp = gcc.Timestamp,
                    }).ToList(),
                    Timestamp = cc.Timestamp
                }).ToList(),
                Timestamp = c.Timestamp
            })
            .ToListAsync();

        return comments;
    }
}