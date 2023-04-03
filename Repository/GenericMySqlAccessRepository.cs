using Microsoft.EntityFrameworkCore;
using UxtrataTask.Context;

namespace UxtrataTask.Repository;

public class GenericMySqlAccessRepository<T> : IGenericMySqlAccessRepository<T> where T : class
{
    private readonly SchoolDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public GenericMySqlAccessRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<T> GetAsync(object Id)
    {
        return await _dbSet.FindAsync(Id);
    }

    public async void Delete(object id)
    {
        Delete(await _dbSet.FindAsync(id));
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entityList)
    {
        _dbSet.RemoveRange(entityList);
    }

    public IQueryable<T> GetQueryable(string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (!string.IsNullOrEmpty(includeProperties))
        {
            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty.Trim()));
        }

        return query;
    }

    public void Insert(T entity)
    {
        _dbSet.Add(entity);
    }

    public void InsertRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void UpdateT(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public DbContext GetContext()
    {
        return _dbContext;
    }
}