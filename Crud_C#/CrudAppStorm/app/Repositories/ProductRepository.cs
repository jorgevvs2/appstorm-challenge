using CrudAppStorm.app.Database;
using CrudAppStorm.app.Domain.Entities;
using CrudAppStorm.app.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CrudAppStorm.app.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository, IDisposable
    {
        protected ProductContext _dbContext;

        public ProductRepository(ProductContext context, IMemoryCache memoryCache) : base(context, memoryCache)
        {
            _dbContext = context;
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
