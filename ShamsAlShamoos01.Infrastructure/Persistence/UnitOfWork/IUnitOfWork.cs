using ShamsAlShamoos01.Infrastructure.Persistence.Repositories;
using ShamsAlShamoos01.Shared.Entities;
using System;

namespace ShamsAlShamoos01.Infrastructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<HistoryRegisterKala01> HistoryRegisterKala01UW { get; }

        IEntityDataBaseTransaction BeginTransaction();

        void Save();
    }
}
