using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.BusinessObjects;
using DataAccess.BusinessObjects.Entities;

namespace DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbSet<T> _objectSet;

        protected readonly DatabaseContext DbContext;

        public Repository(DatabaseContext dbContext)
        {
            DbContext = dbContext;
            _objectSet = DbContext.Set<T>();
        }

        public IEnumerable<T> GetOverview(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? _objectSet.Where(predicate) : _objectSet.AsEnumerable();
        }

        public T GetDetail(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            // _objectSet.Add(entity);
            DbContext.Fields.Add(entity as Field);
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }


        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}