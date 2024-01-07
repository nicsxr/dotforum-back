using DotForum.Domain.Entities;
using DotForum.Domain.Models;
using DotForum.Persistence.DbContexts;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Persistence.Repositories.Implementations;

public class CommunityRepository : CommonRepository<Community>, ICommunityRepository
{
    public CommunityRepository(DotForumDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<CommunityFullInfoModel?> GetByIdFullInfo(string id)
    {
        var community = await DbContext.Communities
            .Where(c => c.Id == id)
            .Include(c => c.Posts)
            .Select(c => new CommunityFullInfoModel
            {
                Community = c,
                TotalPosts = c.Posts != null ? c.Posts.Count : 0
            }).FirstOrDefaultAsync();
        
        return community;
    }
    public async Task<CommunityFullInfoModel?> GetByNormalizedName(string name)
    {
        var community = await DbContext.Communities
            .Where(c => c.NormalizedName == name.ToUpper())
            .Include(c => c.Posts)
            .Select(c => new CommunityFullInfoModel
            {
                Community = c,
                TotalPosts = c.Posts != null ? c.Posts.Count : 0
            }).FirstOrDefaultAsync();
        
        return community;
    }
    
    public async Task<List<CommunityFullInfoModel>> GetAllFollowedByUserAsync(string userId)
    {
        var userCommunities = await DbContext.Users.Include(u => u.FollowedCommunities)
            .Where(uc => uc.Id == userId).SelectMany(u => u.FollowedCommunities).ToListAsync();

        var communities = new List<CommunityFullInfoModel>();

        foreach (var userCommunity in userCommunities)
        {
            var community = await GetByIdFullInfo(userCommunity.CommunityId);

            if (community != null) communities.Add(community);
        }
        

        return communities;
    }
}