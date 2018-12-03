using Invoisys.AppService.Interface;
using Invoisys.Domain.Interface.Service;
using Invoisys.Domain.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Invoisys.AppService
{
    public class AppService<TEntity> : IDisposable, IAppService<TEntity> where TEntity : class
    {
        private readonly IService<TEntity> _serviceApp;
        private readonly IUoW _uow;
        protected AppService(IService<TEntity> serviceApp, IUoW uow)
        {
            _serviceApp = serviceApp;
            _uow = uow;
        }
        public void Add(TEntity obj)
        {
            _serviceApp.Add(obj);
            _uow.Commit();
        }
        public void Delete(TEntity obj)
        {
            _serviceApp.Delete(obj);
            _uow.Commit();
        }
        public void Edit(TEntity obj)
        {
            _serviceApp.Edit(obj);
            _uow.Commit();
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _serviceApp.GetAll();
        }
        public TEntity GetById(int id)
        {
            return _serviceApp.GetById(id);
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> exp)
        {
            return _serviceApp.Get(exp);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            _uow.Dispose();
        }
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            _uow?.Dispose();
        }
    }
}