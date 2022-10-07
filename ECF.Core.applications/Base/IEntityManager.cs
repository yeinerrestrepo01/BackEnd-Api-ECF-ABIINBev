using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECF.Core.applications.Base
{
    public interface IEntityManager<T> where T : class
    {
        void Commit();

        Task CommitAsync();

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        IQueryable<T> GetAll();

        T GetById(int id);

        Task<T> GetByIdAsync(int id);

        void Insert(T entity);

        void InsertRange(IEnumerable<T> entities);

        void SetProp(T item, string propName, Object newValue);

        void SetPropBulk(IEnumerable<T> items, string propName, Object newValue);

        void Update(T entity);

        IEnumerable<T> GetFromSqlQuery(string nativeSqlQuery);

        Task<int> GetSeqSqlQuery(string SQLQuery);

        void UpdateEntityFromDb(T Entity);

        void RemoveEntityFromContext(T Entity);

        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        void BulkInsertAll(T[] entities);

        void BulkUpdateAll(T[] entities);

        void BulkDeleteAll(T[] entities);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        void BulkInsertOrUpdate(T[] entities);
    }
}
