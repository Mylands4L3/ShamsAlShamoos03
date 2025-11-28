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
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);

        /// <summary>
        /// متد مشابه LINQ برای فیلتر و ترتیب
        /// </summary>
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    }
}
