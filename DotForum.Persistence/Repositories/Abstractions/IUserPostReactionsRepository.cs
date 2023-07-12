using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Enums;

namespace DotForum.Persistence.Repositories.Abstractions;

public interface IUserPostReactionsRepository : ICommonRepository<UserPostReaction>
{
    public Task<UserPostReaction?> GetByIdAsync(string userId, string postId);
    public Task<List<UserPostReaction>?> GetByPostId(string postId);
    public int GetVoteCount(string postId, VoteStatusEnum vote);

}