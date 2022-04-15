using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Data.Interfaces
{
    public interface IRepositoryAsync<T> where T : class
    {
        #region CREATE
        ValueTask<EntityEntry<T>> InsertAsync(T entity, CancellationToken cancellationToken = default);
        Task InsertAsync(params T[] entities);
        Task InsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        #endregion
        #region READ
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false);

        Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);
        #endregion
    }
}