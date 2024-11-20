using CarMarketplaceWebApi.Context;
using CarMarketplaceWebApi.Models.Common;
using CarMarketplaceWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarMarketplaceWebApi.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        protected readonly AppDBContext _appDBContext;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
            _dbSet = appDBContext.Set<T>();
        }

        public AppDBContext AppDBContext { get; }

        public async Task CreateAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
           await _appDBContext.SaveChangesAsync(); 
        }

        public async Task DeleteAsync(Guid id)
        {
            T entity= await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _appDBContext.SaveChangesAsync();
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            T entity=await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _appDBContext.SaveChangesAsync();
        }
    }
}
