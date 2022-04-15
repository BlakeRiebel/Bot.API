using DiscordBot.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DiscordBot.Data.UnitofWork
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private DbContext _dbContext;
        private Dictionary<Type, object> _repositories;
        bool disposed;

        public RepositoryFactory(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if(_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(_dbContext);
            return (IRepository<TEntity>)_repositories[type];
        }

        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryAsync<TEntity>(_dbContext);
            return (IRepositoryAsync<TEntity>)_repositories[type];
        }

        public IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryReadOnly<TEntity>(_dbContext);
            return (IRepositoryReadOnly<TEntity>)_repositories[type];
        }

        public IRepositoryReadOnlyAsync<TEntity> GetReadOnlyRepositoryAsync<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryReadOnlyAsync<TEntity>(_dbContext);
            return (IRepositoryReadOnlyAsync<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if(disposing)
                {
                    _dbContext?.Dispose();
                }
            }
            
            disposed = true;
        }
    }
}