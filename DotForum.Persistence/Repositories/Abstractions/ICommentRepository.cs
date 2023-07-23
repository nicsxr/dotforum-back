using DotForum.Domain.Entities;
using DotForum.Domain.Models;

namespace DotForum.Persistence.Repositories.Abstractions;

public interface ICommentRepository : ICommonRepository<Comment>
{
    public Task<List<GetCommentsModel>> GetCommentsByPostId(string postId, int page, int pageSize, string? userId = null);

    public new Task<GetCommentsModel> GetByIdAsync(string commentId, string? userId = null);

    public Task<List<GetCommentsModel>> GetChildComments(string commentId, int page, int pageSize, string? userId = null);

    public Task<int> GetChildCommentsCount(string commentId);
}