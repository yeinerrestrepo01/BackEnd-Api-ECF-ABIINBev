using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace ECF.Core.Repository.Base
{

    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The DbContext
        /// </summary>
        private DbContext _dbContext;

       

        public DbContext getContext()
        {
            return this._dbContext;
        }

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>
        public UnitOfWork(DbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            try
            {
                // Save changes with the default options
                return _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message.ToString(), new Exception(ex.Message.ToString()));
            }
        }
        public async Task<int> CommitAsync()
        {
            try
            {
                // Save changes with the default options
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message.ToString(), new Exception(ex.Message.ToString()));
            }
        }


        public List<Q> RawSqlQuery<Q>(string query, Func<DbDataReader, Q> map)
        {

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                _dbContext.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<Q>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }

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

            if (disposing && _dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
            disposed = true;
        }

        private List<PropertyInfo> GetKeysData(EntityEntry entry)
        {
            var type = entry.Entity.GetType();
            var properties = type.GetProperties().ToList();
            return properties;
        }

        private string GetClassName(EntityEntry entry)
        {
            var type = entry.Entity.GetType();
            var name = type.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .Select(x => ((DisplayNameAttribute)x).DisplayName)
                .DefaultIfEmpty(type.Name)
                .First();

            return name;
        }
    }
}