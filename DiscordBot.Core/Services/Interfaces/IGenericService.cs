using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DiscordBot.Core.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        #region CREATE
        T Insert(T entity);
        void Insert(params T[] entities);
        void Insert(IEnumerable<T> entities);
        #endregion
        #region READ
        IList<T> GetAll(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true);

        IList<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector = null,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true) where TResult : class;

        T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true);
        #endregion
        #region UPDATE
        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);
        #endregion
        #region DELETE
        void Delete(T entity);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
        #endregion
    }
}