using CrudAppStorm.app.Domain.Entities;

namespace CrudAppStorm.app.Repositories.Interface
{
    public interface IProductRepository : IBaseRepository<Product>,IDisposable
    {
    }
}
