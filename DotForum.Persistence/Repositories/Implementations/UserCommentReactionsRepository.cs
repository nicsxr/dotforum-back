using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Enums;
using DotForum.Persistence.DbContexts;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Persistence.Repositories.Implementations;

public class UserCommentReactionsRepository : CommonRepository<UserCommentReaction>, IUserCommentReactionsRepository
{
    public UserCommentReactionsRepository(DotForumDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserCommentReaction?> GetByIdAsync(string userId, string commentId)
    {
        var userCommentReaction = await DbContext.UserCommentReactions
            .Where(ucr => ucr.UserId == userId && ucr.CommentId == commentId).FirstOrDefaultAsync();

        return userCommentReaction; 
    }
    
    public async Task<List<UserCommentReaction>?> GetByPostId(string commentId)
    {
        var userPostReaction = await DbContext.UserCommentReactions
            .Where(ucr => ucr.CommentId == commentId).ToListAsync();

        return userPostReaction; 
    }    
    
    public int GetVoteCount(string commentId, VoteStatusEnum vote)
    {
        return DbContext.UserCommentReactions.Count(ucr => ucr.CommentId == commentId && ucr.VoteStatus == vote);
    }    
}