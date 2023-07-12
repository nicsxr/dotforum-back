using DotForum.Domain.Entities;
using DotForum.Domain.Models;
using DotForum.Persistence.Repositories.Implementations;

namespace DotForum.Persistence.Repositories.Abstractions;

public interface IPostRepository : ICommonRepository<Post>
{
    public Task<List<Post>> GetHomePublicPosts(int pageSize, int page);
    public Task<PostModel> GetPostModelById(string id, string? userId = null);
    public IQueryable<Post> GetQueryableById(string postId);
    public IQueryable<Post> GetQueryableAll();
}