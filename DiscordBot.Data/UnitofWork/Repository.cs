using DiscordBot.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DiscordBot.Data.UnitofWork
{
    public class Repository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        public Repository(DbContext context) : base(context)
        {
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public T GetSingleOrDefault(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            return (orderBy != null) ? orderBy(query).FirstOrDefault() : query.FirstOrDefault();
        }

        public T Insert(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public void Insert(params T[] entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Insert(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Update(params T[] entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Update(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }
}
