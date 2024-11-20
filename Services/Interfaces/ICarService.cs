using CarMarketplaceWebApi.Models;
using CarMarketplaceWebApi.Models.DTOs;
using System.Threading.Tasks;

namespace CarMarketplaceWebApi.Services.Interfaces
{
    public interface ICarService
    {
        Task<List<Car>> GetAllAsync();
        Task<Car> GetByIdAsync(Guid id);
        Task UpdateAsync(UpdateCarDTO updateCarDTO);
        Task DeleteAsync(Guid id);
        Task CreateAsync(CarDTO carDTO);
        Task ApproveRequest(Guid id, string action);
        Task<IEnumerable<Car>> GetApprovedCars();
        Task<IEnumerable<Car>> GetPendingCars();
        Task<IEnumerable<UserCarsDTO>> GetAllCurrrentUserCars();
        Task AddFavorites(Guid carId);
        Task RemoveFromFavorites(Guid carId);
        Task<List<Car>> GetFavoritesCars();
        Task ComplainCar(ComplainDTO complainDTO);
    }
}
