

using ShamsAlShamoos01.Infrastructure.Persistence.Contexts;
 using Microsoft.EntityFrameworkCore.Storage;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    public class EntityDataBaseTransaction : IEntityDataBaseTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EntityDataBaseTransaction(ApplicationDbContext context)//یک نمونه از دیتابیس اینیشیال میذاریم-
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void RollBack()
        {
            _transaction.Rollback();
        }


        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
