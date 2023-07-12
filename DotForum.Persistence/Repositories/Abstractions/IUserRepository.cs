using DotForum.Domain.Entities;

namespace DotForum.Persistence.Repositories.Abstractions;

public interface IUserRepository : ICommonRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetUserByUsernameAsync(string username);
}