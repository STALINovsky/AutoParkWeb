using System.Threading.Tasks;
using AutoParkData.Model.Base;

namespace AutoParkData.Repositories.Base
{
    /// <summary>
    /// Base CRUD methods for repository
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> where T : Entity
    {
        Task Add(T entity);
        Task<T> Get(int id);
        Task Update(T entity);
        Task Delete(int id);
    }
}