using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Data.Interfaces
{
    public interface IUnitOfWork : IRepositoryFactory, IDisposable
    {
        int Commit(bool autoHistory = false);
        Task<int> CommitAsync(bool autoHistory = false);
        void Rollback();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}