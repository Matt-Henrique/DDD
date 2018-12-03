using Invoisys.Domain.Interface.Repository;
using Invoisys.Domain.Interface.Service;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Invoisys.Domain.Service
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> _repository;
        protected Service(IRepository<TEntity> repository)
        {
            _repository = repository;
        }
        public void Add(TEntity obj)
        {
            _repository.Add(obj);
        }
        public void Delete(TEntity obj)
        {
            _repository.Delete(obj);
        }
        public void Edit(TEntity obj)
        {
            _repository.Edit(obj);
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> exp)
        {
            return _repository.Get(exp);
        }
        public IQueryable<TEntity> Get()
        {
            return _repository.Get();
        }
        public IQueryable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }
        public TEntity GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}