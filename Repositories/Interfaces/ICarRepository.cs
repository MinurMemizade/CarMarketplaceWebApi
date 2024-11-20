using CarMarketplaceWebApi.Models;
using CarMarketplaceWebApi.Models.DTOs;
using CarMarketplaceWebApi.Models.Enums;
using System.Threading.Tasks;

namespace CarMarketplaceWebApi.Repositories.Interfaces
{
    public interface ICarRepository:IRepository<Car>
    {
        Task<IEnumerable<Car>> GetCarsByStatusAsync(int status);
        Task<IEnumerable<UserCarsDTO>> GetCarsByUserId(Guid id);
        Task AddFavorite(Guid carId, Guid userId);
        Task RemoveFromFavorite(Guid carId, Guid userId);
        Task<List<Car>> GetFavoritesCars(Guid userId);
    }
}
