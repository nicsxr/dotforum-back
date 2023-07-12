using DotForum.Domain.Entities;
using DotForum.Domain.Models;

namespace DotForum.Persistence.Repositories.Abstractions;

public interface ICommunityRepository : ICommonRepository<Community>
{
    public Task<CommunityFullInfoModel?> GetByIdFullInfo(string id);
    public Task<List<CommunityFullInfoModel>> GetAllFollowedByUserAsync(string userId);
}