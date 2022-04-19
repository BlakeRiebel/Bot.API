using DiscordBot.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data.UnitofWork
{
    public class UnitOfWork<TContext> : RepositoryFactory, IUnitOfWork<TContext>
        where TContext : DbContext
    {
        public UnitOfWork(TContext context) : base(context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TContext Context { get; }

        public int Commit(bool autoHistory = false)
        {
            if (autoHistory) Context.EnsureAutoHistory();

            return Context.SaveChanges();
        }

        public async Task<int> CommitAsync(bool autoHistory = false)
        {
            if (autoHistory) Context.EnsureAutoHistory();

            return await Context.SaveChangesAsync();
        }

        public void Rollback()
        {
            Context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }
    }
}
