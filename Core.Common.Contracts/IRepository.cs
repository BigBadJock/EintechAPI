using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    /// <summary>
    /// Interface for base generic repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T: class, IModel, new()
    {
        DbSet<T> DbSet { get; }

        #region get by id
        Task<T> GetById(long id);
        #endregion

        IQueryable<T> GetAll();

        #region update
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Delete(Expression<Func<T, bool>> where);
        Task<bool> DeleteById(int id);
        #endregion
    }
}
