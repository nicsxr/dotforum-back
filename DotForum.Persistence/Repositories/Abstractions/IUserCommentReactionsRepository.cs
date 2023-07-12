using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Enums;

namespace DotForum.Persistence.Repositories.Abstractions;

public interface IUserCommentReactionsRepository : ICommonRepository<UserCommentReaction>
{
    public Task<UserCommentReaction?> GetByIdAsync(string userId, string commentId);
    public Task<List<UserCommentReaction>?> GetByPostId(string commentId);
    public int GetVoteCount(string commentId, VoteStatusEnum vote);
}