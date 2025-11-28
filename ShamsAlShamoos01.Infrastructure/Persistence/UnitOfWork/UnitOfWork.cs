using ShamsAlShamoos01.Infrastructure.Persistence.Contexts;
using ShamsAlShamoos01.Infrastructure.Persistence.Repositories;
using ShamsAlShamoos01.Shared.Entities;
using System.Collections.Concurrent;

namespace ShamsAlShamoos01.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ConcurrentDictionary<System.Type, Lazy<object>> _repositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // متد عمومی برای دسترسی به repositoryها (lazy load و thread-safe)
        private IGenericRepository<T> GetRepository<T>() where T : class
        {
            var repo = _repositories.GetOrAdd(
                typeof(T),
                t => new Lazy<object>(() => new GenericClass<T>(_context)) // GenericClass<T> همان IGenericRepository<T> را پیاده‌سازی می‌کند
            );
            return (IGenericRepository<T>)repo.Value;
        }

        // Property مطابق با interface
        public IGenericRepository<HistoryRegisterKala01> HistoryRegisterKala01UW => GetRepository<HistoryRegisterKala01>();

        // متدهای اینترفیس
        public IEntityDataBaseTransaction BeginTransaction() => new EntityDataBaseTransaction(_context);

        public void save() => _context.SaveChanges();

        public Task<int> SaveAsync() => _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        {
            _repositories.Clear();
            await _context.DisposeAsync();
        }

        public void Dispose()
        {
            _repositories.Clear();
            _context.Dispose();
        }
    }
}
