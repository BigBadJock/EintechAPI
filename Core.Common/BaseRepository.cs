using Ardalis.GuardClauses;
using Core.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Core.Common
{

    /// <summary>
    /// Base Generic Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IModel, new()
    {

        private DbContext dataContext;
        private ILogger<Common.BaseRepository<T>> logger;
        private readonly DbSet<T> dbset;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataContext"></param>
        protected BaseRepository(DbContext dataContext, ILogger<BaseRepository<T>> logger)
        {
            this.logger = logger;
            this.logger.LogInformation($"Creating Repository {this.GetType().Name}");
            this.dataContext = dataContext;
            dbset = DataContext.Set<T>();
        }

        protected DbContext DataContext
        {
            get { return dataContext; }
        }

        public DbSet<T> DbSet => dbset;

        public virtual async Task<T> Add(T entity)
        {
            try
            {
                Guard.Against.Null(entity, nameof(entity));
                entity.LastUpdated = DateTime.Now;
                entity.Created = DateTime.Now;
                var added = dbset.Add(entity);
                _ = await dataContext.SaveChangesAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Repository: {this.GetType().Name} added new entity");
                return added.Entity;
            }
            catch (ArgumentNullException)
            {
                this.logger.LogError($"Repository: {this.GetType().Name} tried to add a null entity");
                throw;
            }
            catch (DbUpdateException e)
            {
                this.logger.LogError($"Repository: {this.GetType().Name} failed throwing exception: {e} when trying to add an entity", e);
                throw;
            }
        }

        public virtual async Task<bool> Delete(T entity)
        {
            try
            {
                Guard.Against.Null(entity, nameof(entity));
                dbset.Remove(entity);
                await dataContext.SaveChangesAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Repository: {this.GetType().Name} deleted then entity: {JsonConvert.SerializeObject(entity)}");

                return true;
            }
            catch (DbUpdateException e)
            {
                this.logger.LogError($"Repository: {this.GetType().Name} failed throwing exception: {e} when trying to delete an entity : {JsonConvert.SerializeObject(entity)}", e);
                return false;
            }

        }

        public virtual async Task<bool> Delete(Expression<Func<T, bool>> where)
        {
            try
            {
                IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
                foreach (T obj in objects)
                {
                    dbset.Remove(obj);
                    this.logger.LogInformation($"Repository: {this.GetType().Name} deleting entity: {JsonConvert.SerializeObject(obj)}");
                }
                await dataContext.SaveChangesAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Repository: {this.GetType().Name} deleting multiple entities successful");
                return true;
            }
            catch (DbUpdateException e)
            {
                this.logger.LogError($"Repository: {this.GetType().Name} failed throwing exception: {e} when trying to deleting multiple entries", e);
                throw;
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbset;
        }


        public virtual async Task<T> GetById(long id)
        {
            T result = await dbset.Where(s => s.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
            this.logger.LogInformation($"Repository: {this.GetType().Name} retrieving by Id: {id} value: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        public virtual async Task<T> Update(T entity)
        {
            try
            {
                Guard.Against.Null(entity, nameof(entity));
                dbset.Attach(entity);
                dataContext.Entry(entity).State = EntityState.Modified;
                await dataContext.SaveChangesAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Repository: {this.GetType().Name} updating entity: {JsonConvert.SerializeObject(entity)}");
                return entity;
            }
            catch (DbUpdateException e)
            {
                this.logger.LogError($"Repository: {this.GetType().Name} failed throwing exception: {e} when trying to update entity: {JsonConvert.SerializeObject(entity)}", e);
                throw;
            }
        }

        public virtual async Task<bool> DeleteById(int id)
        {
            try
            {
                T obj = dbset.Where<T>(x => x.Id == id).FirstOrDefault();
                if (obj != null)
                {
                    dbset.Remove(obj);
                    this.logger.LogInformation($"Repository: {this.GetType().Name} deleting entity: {JsonConvert.SerializeObject(obj)}");
                    await dataContext.SaveChangesAsync().ConfigureAwait(false);
                    this.logger.LogInformation($"Repository: {this.GetType().Name} deleting multiple entities successful");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (DbUpdateException e)
            {
                this.logger.LogError($"Repository: {this.GetType().Name} failed throwing exception: {e} when trying to deleting entry", e);
                throw;
            }
        }
    }
}
