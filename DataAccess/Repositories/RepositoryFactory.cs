using DataAccess.BusinessObjects;

namespace DataAccess.Repositories
{
    public class RepositoryFactory
    {
        protected static DatabaseContext DbContext = new DatabaseContext();

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(DbContext);
        }

        public static DatabaseContext SetDbContext(DatabaseContext dbContext)
        {
            var oldDbContext = DbContext;
            DbContext = dbContext;
            return oldDbContext;
        }
    }
}