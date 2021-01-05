using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetOverview(Expression<Func<T, bool>> predicate=null);

        T GetDetail(Expression<Func<T, bool>> predicate);
        
        void Add(T entity);

        void Delete(T entity);

        void SaveChanges();
    }
}
