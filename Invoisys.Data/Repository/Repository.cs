using Invoisys.Domain.Interface.Repository;
using Invoisys.Infrastructure.Data.Context;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Invoisys.Infrastructure.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected InvoisysContext Context { get; set; }
        protected Repository(InvoisysContext context)
        {
            Context = context;
        }
        public void Add(TEntity obj)
        {
            Context.Set<TEntity>().Add(obj);
        }
        public void Delete(TEntity obj)
        {
            Context.Set<TEntity>().Remove(obj);
        }
        public void Edit(TEntity obj)
        {
            Context.Entry(obj).State = EntityState.Modified;
        }
        public IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>();
        }
        public TEntity GetById(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> exp)
        {
            return Context.Set<TEntity>().Where(exp);
        }
        public IQueryable<TEntity> Get()
        {
            return Context.Set<TEntity>();
        }
    }
}