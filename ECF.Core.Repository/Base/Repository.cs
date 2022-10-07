using ECF.Core.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ECF.Core.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region ..:: Constants ::..

        private const string SQL_DATA_PROVIDER = "System.Data.SqlClient";

        #endregion

        #region ..:: Variables ::..

        protected DbContext _context;

        #endregion

        #region ..:: Properties ::..

        /// <summary>
        /// Gets the DbContext
        /// </summary>
        public DbContext Context
        {
            get { return _context; }
        }

        public string Provider
        {
            get { return SQL_DATA_PROVIDER; }
        }

        #endregion

        #region ..:: Constructor ::..

        public Repository(DbContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        #region ..:: Public Methods ::..

        /// <summary>
        /// Delete a entity
        /// </summary>
        virtual public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        /// <summary>
        /// Delete a list of entities
        /// </summary>
        virtual public void DeleteRange(IEnumerable<T> items)
        {
            _context.Set<T>().RemoveRange(items);
        }

        /// <summary>
        /// Get iqueryable
        /// </summary>
        virtual public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual T GetById(int id)
        {
            T t = _context.Set<T>().Find(id);
            if (t == null)
                return null;
            _context.Entry<T>(t).Reload();
            return t;
        }

        public virtual T GetById(object[] values)
        {
            T t = this._context.Set<T>().Find(values);
            if (t == null)
                return null;
            this._context.Entry<T>(t).Reload();
            return t;
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            T t = await _context.Set<T>().FindAsync(id);
            if (t == null)
                return null;
            await _context.Entry<T>(t).ReloadAsync();
            return t;
        }

        public async virtual Task<T> GetByIdAsync(object[] values)
        {
            T t = await this._context.Set<T>().FindAsync(values);
            if (t == null)
                return null;
            await this._context.Entry<T>(t).ReloadAsync();
            return t;
        }

        /// <summary>
        /// Insert a entity
        /// </summary>
        virtual public void Insert(T item)
        {
            _context.Set<T>().Add(item);
        }

        /// <summary>
        /// Insert a list of entities
        /// </summary>
        virtual public void InsertRange(IEnumerable<T> items)
        {
            _context.Set<T>().AddRange(items);
        }

        /// <summary>
        /// Set property "propName" to new value
        /// </summary>
        public void SetProp(T item, string propName, Object newValue)
        {
            Type theType = item.GetType();
            List<PropertyInfo> properties = new List<PropertyInfo>(theType.GetProperties());
            foreach (PropertyInfo prop in properties)
            {
                if (prop.Name == propName)
                {
                    prop.SetValue(item, newValue);
                }
            }
        }

        /// <summary>
        /// Set property "propName" to new value
        /// </summary>
        public void SetPropBulk(IEnumerable<T> items, string propName, Object newValue)
        {
            Type theType = items.First().GetType();
            List<PropertyInfo> properties = new List<PropertyInfo>(theType.GetProperties());
            foreach (PropertyInfo prop in properties)
            {
                if (prop.Name == propName)
                {
                    items.ToList().ForEach(x => prop.SetValue(x, newValue));
                }
            }
        }

        public void Update(T item)
        {
            _context.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            _context.Update(item);
        }

        bool disposed = false;

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
            }
            disposed = true;
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
                query = includes(query);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public void BulkInsertAll(T[] entities)
        {
            ((DROfficeContext)_context).BulkInsertAll(entities);
        }

        public void BulkUpdateAll(T[] entities)
        {
          
          ((DROfficeContext)_context).BulkUpdateAll(entities);

        }

        public void BulkDeleteAll(T[] entities)
        {
            ((DROfficeContext)_context).BulkDeleteAll(entities);
        }

        public void BulkInsertOrUpdate(T[] entities)
        {
            ((DROfficeContext)_context).BulkInsertOrUpdateAll(entities);
        }

        #endregion
    }
}
