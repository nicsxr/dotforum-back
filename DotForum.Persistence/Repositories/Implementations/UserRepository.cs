using DotForum.Domain.Entities;
using DotForum.Persistence.DbContexts;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Persistence.Repositories.Implementations;

public class UserRepository : CommonRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(DotForumDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<ApplicationUser?> GetUserByUsernameAsync(string username)
    {
        return await DbSet.FirstOrDefaultAsync(user => user.UserName == username);
    }
}