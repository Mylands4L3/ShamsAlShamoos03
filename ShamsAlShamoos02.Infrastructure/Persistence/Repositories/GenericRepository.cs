using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ShamsAlShamoos01.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    public class GenericClass<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _table;
        private IDbContextTransaction _transaction;

        public GenericClass(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _table = _context.Set<T>();
        }

        #region CRUD
        public async Task AddAsync(T entity) => await _table.AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities) => await _table.AddRangeAsync(entities);
        public void Update(T entity) => _table.Update(entity);
        public void Remove(T entity) => _table.Remove(entity);
        public void RemoveRange(IEnumerable<T> entities) => _table.RemoveRange(entities);
        public async Task<T> GetByIdAsync(object id) => await _table.FindAsync(id);
        #endregion

        #region GetAll Overloads
        public async Task<IEnumerable<T>> GetAllAsync() => await _table.ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {     
                return await GetAllAsync();
            }
        
            return await _table.Where(filter).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            IQueryable<T> query = _table;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            int skip,
            int take)
        {
            IQueryable<T> query = _table;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip(skip).Take(take);

            return await query.ToListAsync();
        }

        #endregion

        #region Transaction
        public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();
        public async Task CommitAsync() => await _transaction?.CommitAsync();
        public async Task RollbackAsync() => await _transaction?.RollbackAsync();
        #endregion
    }
}
