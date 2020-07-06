using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IDataService<T> where T : class, IModel, new()
    {
        Task<T> Add(T model);
        Task<T> Update(T model);
        Task<bool> Delete(int id);
        Task<T> GetById(int id);
        IQueryable<T> GetAll();
    }
}
