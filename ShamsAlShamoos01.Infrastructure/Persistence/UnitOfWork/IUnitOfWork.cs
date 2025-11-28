
using ShamsAlShamoos01.Infrastructure.Persistence.Repositories;
using ShamsAlShamoos01.Shared.Entities;


namespace ShamsAlShamoos01.Infrastructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {

        IGenericRepository<HistoryRegisterKala01> HistoryRegisterKala01UW { get; }

        IEntityDataBaseTransaction BeginTransaction();

        void save();
        void Dispose();
    }
}
