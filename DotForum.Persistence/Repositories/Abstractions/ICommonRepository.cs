namespace DotForum.Persistence.Repositories.Abstractions;

public interface ICommonRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(string? id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}