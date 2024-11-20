using CarMarketplaceWebApi.Models.Common;

namespace CarMarketplaceWebApi.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T entity);
        Task CreateAsync(T entity);
    }
}
