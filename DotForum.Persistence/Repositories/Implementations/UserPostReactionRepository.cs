using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Enums;
using DotForum.Persistence.DbContexts;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Persistence.Repositories.Implementations;

public class UserPostReactionRepository : CommonRepository<UserPostReaction>, IUserPostReactionsRepository
{
    public UserPostReactionRepository(DotForumDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserPostReaction?> GetByIdAsync(string userId, string postId)
    {
        var userPostReaction = await DbContext.UserPostReactions
            .Where(upr => upr.UserId == userId && upr.PostId == postId).FirstOrDefaultAsync();

        return userPostReaction; 
    }
    
    public async Task<List<UserPostReaction>?> GetByPostId(string postId)
    {
        var userPostReaction = await DbContext.UserPostReactions
            .Where(upr => upr.PostId == postId).ToListAsync();

        return userPostReaction; 
    }    
    
    public int GetVoteCount(string postId, VoteStatusEnum vote)
    {
        return DbContext.UserPostReactions.Count(upr => upr.PostId == postId && upr.VoteStatus == vote);
    }    
}