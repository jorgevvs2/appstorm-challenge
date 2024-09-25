using CrudAppStorm.app.Database;
using CrudAppStorm.app.Domain.Constants;
using CrudAppStorm.app.Domain.Entities.Interfaces;
using CrudAppStorm.app.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace CrudAppStorm.app.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBase 
    {
        protected readonly ProductContext _context;
        protected readonly IMemoryCache _cache;

        public BaseRepository(ProductContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        public async Task Save(int id = 0)
        {
            await _context.SaveChangesAsync();
            await CleanCache($"{ProductConstants.productByIdKey}{id}", ProductConstants.allProductKey);
        }
        public async Task CleanCache(params string[] removeStrings)
        {
            foreach(var removeString in removeStrings)
            {
                _cache.Remove(removeString);
            }
        }

        public async Task Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            await Save(obj.Id);
        }
        public async Task Delete(int id)
        {
            var obj = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(obj);
            await Save(obj.Id);
        }
        public async Task<T?> GetById(int id)
        {
            return await _cache.GetOrCreateAsync<T>(ProductConstants.productByIdKey + id, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.Low);
                Thread.Sleep(1000);
                var entity = _context.Set<T>().Find(id);
                return Task.FromResult(_context.Set<T>().Find(id)!);
            });
        }
        public async Task<IEnumerable<T>?> GetAll()
        {
            return await _cache.GetOrCreateAsync(ProductConstants.allProductKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(3);
                entry.SetPriority(CacheItemPriority.High);
                Thread.Sleep(1000);
                return _context.Set<T>().ToListAsync();
            });
        }
        public async Task Insert(T obj)
        {
            _context.Set<T>().Add(obj);
            await Save(obj.Id);
        }

        public async Task<bool> Exists(int id)
        {
            return _context.Product.Any(x => x.Id == id);
        }
    }
}
