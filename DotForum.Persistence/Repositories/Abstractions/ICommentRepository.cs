using DotForum.Domain.Entities;

namespace DotForum.Persistence.Repositories.Abstractions;

public interface ICommentRepository : ICommonRepository<Comment>
{
    public Task<List<Comment>> GetCommentsByPostId(string postId, int page, int pageSize);

    public new Task<Comment> GetByIdAsync(string commentId);
    public Task<List<Comment>> GetChildComments(string commentId, int page, int pageSize);

    public Task<int> GetChildCommentsCount(string commentId);
}