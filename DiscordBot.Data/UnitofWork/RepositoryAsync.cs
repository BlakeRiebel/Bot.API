using DiscordBot.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Data.UnitofWork
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public RepositoryAsync(DbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }

        public ValueTask<EntityEntry<T>> InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            return _dbSet.AddAsync(entity, cancellationToken);
        }

        public Task InsertAsync(params T[] entities)
        {
            return _dbSet.AddRangeAsync(entities);
        }

        public Task InsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            return await ((orderBy != null) ? orderBy(query).FirstOrDefaultAsync() : query.FirstOrDefaultAsync());
        }

        public Task<IPaginate<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20, bool enableTracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            if (orderBy != null)
                return orderBy(query).Select(selector).ToPaginateAsync(index, size, 0, cancellationToken);

            return query.Select(selector).ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);

            return query.ToPaginateAsync(index, size, 0, cancellationToken);
        }
    }
}
