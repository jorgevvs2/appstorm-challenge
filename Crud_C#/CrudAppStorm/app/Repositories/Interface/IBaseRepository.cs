using CrudAppStorm.app.Domain.Constants;

namespace CrudAppStorm.app.Repositories.Interface
{
    public interface IBaseRepository <T>
    {
        Task Save(int id = 0);
        Task Delete(int id);
        Task Update(T obj);
        Task<IEnumerable<T>?> GetAll();
        Task<T?> GetById(int id);
        Task Insert(T obj);
        Task<bool> Exists(int id);
    }
}
