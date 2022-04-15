using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data;
using DiscordBot.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DiscordBot.Core.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        IUnitOfWork<DiscordBotDBContext> _unitOfWork;
        IRepository<T> _repository;

        #region Constructor
        public GenericService(IUnitOfWork<DiscordBotDBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<T>();
        }
        #endregion
        #region CREATE
        public T Insert(T entity)
        {
            entity = _unitOfWork.GetRepository<T>().Insert(entity);
            _unitOfWork.Commit();
            return entity;
        }

        public void Insert(params T[] entities)
        {
            _unitOfWork.GetRepository<T>().Insert(entities);
            _unitOfWork.Commit();
        }

        public void Insert(IEnumerable<T> entities)
        {
            _unitOfWork.GetRepository<T>().Insert(entities);
            _unitOfWork.Commit();
        }
        #endregion
        #region READ
        public IList<T> GetAll(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true)
        {
            return _repository.GetList(predicate, orderBy, include, 0, int.MaxValue, enableTracking).Items;
        }

        public IList<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector = null, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true) where TResult : class
        {
            return _repository.GetList(selector, predicate, orderBy, include, 0, int.MaxValue, enableTracking).Items;
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true)
        {
            return _repository.SingleOrDefault(predicate, orderBy, include, enableTracking);
        }
        #endregion
        #region UPDATE
        public void Update(T entity)
        {
            _unitOfWork.GetRepository<T>().Update(entity);
            _unitOfWork.Commit();
        }

        public void Update(params T[] entities)
        {
            _unitOfWork.GetRepository<T>().Update(entities);
            _unitOfWork.Commit();
        }

        public void Update(IEnumerable<T> entities)
        {
            _unitOfWork.GetRepository<T>().Update(entities);
            _unitOfWork.Commit();
        }
        #endregion
        #region DELETE
        public void Delete(T entity)
        {
            _unitOfWork.GetRepository<T>().Delete(entity);
            _unitOfWork.Commit();
        }

        public void Delete(params T[] entities)
        {
            _unitOfWork.GetRepository<T>().Delete(entities);
            _unitOfWork.Commit();
        }

        public void Delete(IEnumerable<T> entities)
        {
            _unitOfWork.GetRepository<T>().Delete(entities);
            _unitOfWork.Commit();
        }
        #endregion
    }
}