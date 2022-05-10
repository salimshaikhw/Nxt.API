using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nxt.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<int> CountAsync();
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(T entity);
        IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includeProperties);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match = null);
        Task<T> FindAsync(Expression<Func<T, bool>> match, bool trackChanges = false);
        Task<T> FindByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> match = null);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query(Expression<Func<T, bool>> predicate = null, bool ignoreGlobalQueryFilter = false, params Expression<Func<T, object>>[] includes);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
    }
}
