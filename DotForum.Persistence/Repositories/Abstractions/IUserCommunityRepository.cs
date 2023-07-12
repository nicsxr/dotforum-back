using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Models;

namespace DotForum.Persistence.Repositories.Abstractions;

public interface IUserCommunityRepository : ICommonRepository<UserCommunity>
{
    public Task<UserCommunity?> GetByIdAsync(string userId, string communityId);
}