using DiscordBot.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Data.UnitofWork
{
    public class RepositoryReadOnlyAsync<T> : RepositoryAsync<T>, IRepositoryReadOnlyAsync<T> where T : class
    {
        public RepositoryReadOnlyAsync(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IPaginate<T>> GetList(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20)
        {
            return await base.GetListAsync(predicate, orderBy, include, index, size, false);
        }

        public async Task<IPaginate<TResult>> GetList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20, CancellationToken cancellationToken = default, bool ignoreQueryFilters = false) where TResult : class
        {
            return await base.GetListAsync(selector, predicate, orderBy, include, index, size, false, ignoreQueryFilters, cancellationToken);
        }

        public async Task<T> SingleOrDefault(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            return await base.SingleOrDefaultAsync(predicate, orderBy, include, false);
        }
    }
}