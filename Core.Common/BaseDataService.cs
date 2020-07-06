using Core.Common.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{

    /// <summary>
    /// Base Data Generic Service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDataService<T> : IDataService<T> where T : class, IModel, new()
    {
        protected IRepository<T> repository;
        protected ILogger<T> logger;

        public BaseDataService(IRepository<T> repository, ILogger<T> logger)
        {
            this.repository = repository;
            this.logger = logger;
            this.logger.LogInformation($"Creating DataService {this.GetType().Name}");

        }

        public async Task<T> Add(T model)
        {
            try
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} adding new entity");
                return await this.repository.Add(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"DataService: {this.GetType().Name} error adding new entity: ${ex.Message}");
                throw ex;
            }
            finally
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} exiting add new entity");
            }

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} deleting entity");
                return await this.repository.DeleteById(id);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"DataService: {this.GetType().Name} error deleting entity: ${ex.Message}");
                throw ex;
            }
            finally
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} exiting delete entity");
            }
        }

        public IQueryable<T> GetAll()
        {
            try
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} getting all");
                return this.repository.GetAll();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"DataService: {this.GetType().Name} error getting all: ${ex.Message}");
                throw ex;
            }
            finally
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} exiting getting all");
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} getting by id: ${id.ToString()}");
                return await this.repository.GetById(id);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"DataService: {this.GetType().Name} error getting by id: ${ex.Message}");
                throw ex;
            }
            finally
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} exiting getting by id: ${id.ToString()}");
            }

        }

        public async Task<T> Update(T model)
        {
            try
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} updating entity");
                return await this.repository.Update(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"DataService: {this.GetType().Name} error updating entity: ${ex.Message}");
                throw ex;
            }
            finally
            {
                this.logger.LogInformation($"DataService: {this.GetType().Name} exiting updating entity");
            }
        }
    }
}
