using Microsoft.EntityFrameworkCore;
using Nxt.Common.Exceptions;
using Nxt.Repositories.DataContext;
using Nxt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nxt.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            _ = await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            _ = await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _ = await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            _ = await _context.SaveChangesAsync();
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match = null)
        {
            if (match == null)
                return await _context.Set<T>().ToListAsync();
            else
                return await _context.Set<T>().Where(match).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> match = null)
        {
            if (match == null)
                return await _context.Set<T>().AsNoTracking().ToListAsync();
            else
                return await _context.Set<T>().AsNoTracking().Where(match).ToListAsync();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                throw new RepositoryException($"Entity {nameof(T)} not found witd id: {id} ");
            return entity;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match, bool trackChanges = false)
        {
            if (trackChanges)
                return await _context.Set<T>().SingleOrDefaultAsync(match);
            else
                return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(match);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            _ = await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CountAsync() => await _context.Set<T>().AsNoTracking().CountAsync();

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().AsNoTracking().Where(predicate).AnyAsync();

        public IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? page = null,
            int? pageSize = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate = null, bool ignoreGlobalQueryFilter = false, params Expression<Func<T, object>>[] includes)
        {

            var queryable = ignoreGlobalQueryFilter ? _context.Set<T>().IgnoreQueryFilters().AsQueryable() : _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                queryable = includes.Aggregate(queryable, (set, inc) => set.Include(inc));
            }
            if (predicate != null)
            {
                queryable = queryable.Where(predicate).AsQueryable();
            }

            return queryable.AsQueryable();
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(@"call update_all_symbols(@_old_symbol, @_new_symbol)", parameters: parameters);
        }
    }
}
