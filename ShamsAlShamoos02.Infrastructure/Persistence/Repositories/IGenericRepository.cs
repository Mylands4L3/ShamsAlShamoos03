using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(object id);

        T GetById(object id);

        // ============================
        // متدهای Get با overloading
        // ============================

        // فقط فیلتر
        IEnumerable<T> Get(Expression<Func<T, bool>> filter);

        // فقط ترتیب
        IEnumerable<T> Get(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

        // فیلتر و ترتیب
        IEnumerable<T> Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

        // بدون فیلتر و ترتیب (همان GetAll قبلی)
        IEnumerable<T> GetAll();
    }
}
