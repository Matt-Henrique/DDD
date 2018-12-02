using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Invoisys.AppService.Interface
{
    public interface IAppService<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        void Delete(TEntity obj);
        void Edit(TEntity obj);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> exp);
        void Dispose();
    }
}