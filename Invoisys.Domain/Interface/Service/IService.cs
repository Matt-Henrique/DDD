﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Invoisys.Domain.Interface.Service
{
    public interface IService<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        void Delete(TEntity obj);
        void Edit(TEntity obj);
        TEntity GetById(int id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> exp);
        IQueryable<TEntity> Get();
    }
}