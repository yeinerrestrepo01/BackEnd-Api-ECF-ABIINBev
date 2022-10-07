using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECF.Core.Repository.Base
{

    public interface IRepository<T> : IDisposable where T : class
    {
        DbContext Context { get; }

        string Provider { get; }

        void Delete(T item);

        void DeleteRange(IEnumerable<T> items);

        IQueryable<T> GetAll();

        T GetById(int id);

        T GetById(object[] values);

        Task<T> GetByIdAsync(object[] values);

        Task<T> GetByIdAsync(int id);

        void Insert(T item);

        void InsertRange(IEnumerable<T> items);

        void Update(T item);

        void SetProp(T item, string propName, Object newValue);

        void SetPropBulk(IEnumerable<T> items, string propName, Object newValue);

        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        void BulkInsertAll(T[] entities);

        void BulkUpdateAll(T[] entities);

        void BulkDeleteAll(T[] entities);

        void BulkInsertOrUpdate(T[] entities);
    }
}
