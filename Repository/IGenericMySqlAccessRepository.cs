using Microsoft.EntityFrameworkCore;

namespace UxtrataTask.Repository;

public interface IGenericMySqlAccessRepository<T> {
    Task<T> GetAsync(object Id);
    Task<List<T>> GetAllAsync();
    IQueryable<T> GetQueryable(string includeProperties = "");
    Task SaveAsync();
    void Delete(object Id);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entityList);
    void UpdateT(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Insert(T entity);
    void InsertRange(IEnumerable<T> entities);
    DbContext GetContext();
}