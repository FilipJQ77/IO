using System.Data.Entity;
using DataAccess.BusinessObjects;

namespace DataAccess.Repositories
{
    public class RepositoryFactory
    {
        protected static readonly DatabaseContext DbContext = new DatabaseContext();

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(DbContext);
        }
    }
}