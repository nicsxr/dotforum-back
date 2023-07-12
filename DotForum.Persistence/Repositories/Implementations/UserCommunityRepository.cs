using DotForum.Domain.Entities;
using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Models;
using DotForum.Persistence.DbContexts;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Persistence.Repositories.Implementations;

public class UserCommunityRepository : CommonRepository<UserCommunity>, IUserCommunityRepository
{
    public UserCommunityRepository(DotForumDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<UserCommunity?> GetByIdAsync(string userId, string communityId)
    {
        var community = await DbContext.UserCommunities
            .Where(uc => uc.UserId == userId && uc.CommunityId == communityId).FirstOrDefaultAsync();

        return community; 
    }
}