using ShamsAlShamoos01.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    public class GenericClass<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _table;

        public GenericClass(ApplicationDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public void Create(T entity) => _table.Add(entity);

        public void Update(T entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _table.Attach(entity);
            _table.Remove(entity);
        }

        public void DeleteById(object id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }

        public T GetById(object id) => _table.Find(id);

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _table;
            if (filter != null)
                query = query.Where(filter);
            return query.ToList();
        }

        public IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _table;
            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);
            return query.ToList();
        }
    }
}
