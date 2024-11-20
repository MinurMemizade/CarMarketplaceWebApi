using Azure.Core;
using CarMarketplaceWebApi.Context;
using CarMarketplaceWebApi.Models;
using CarMarketplaceWebApi.Models.DTOs;
using CarMarketplaceWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarMarketplaceWebApi.Repositories.Implementations
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(AppDBContext appDBContext) : base(appDBContext)
        {
        }

        public async Task<IEnumerable<Car>> GetCarsByStatusAsync(int status)
        {
            return await _dbSet.Where(car => (int)car.eStatus == status).ToListAsync();
        }

        public async Task<IEnumerable<UserCarsDTO>> GetCarsByUserId(Guid id)
        {
            return await _dbSet.Where(car => car.UserId == id)
                .Select(car => new UserCarsDTO
                {
                    Id = car.Id,
                    UserId = car.UserId,
                    eStatus = car.eStatus,
                    Make = car.Make,
                    Model = car.Model,
                    Mileage = car.Mileage,
                    Price = car.Price,
                    Class = car.Class,
                    ImageUrls = car.ImageUrls,
                    IsNew = car.IsNew,
                    SellerEmail = car.SellerEmail,
                    SellerName = car.SellerName,
                    Year = car.Year,
                    createdTime = car.createdTime,
                    soldTime = car.soldTime
                }).ToListAsync();
        }

        public async Task AddFavorite(Guid carId, Guid userId)
        {
            var existingFav = await _appDBContext.Favorites
                .AnyAsync(f=>f.UserId==userId && f.CarId==carId);

            if(!existingFav)
            {
                var favorite = new Favorites
                {
                    UserId = userId,
                    CarId = carId,
                };

                await _appDBContext.Favorites.AddAsync(favorite);
                await _appDBContext.SaveChangesAsync();
            };
        }

        public async Task RemoveFromFavorite(Guid carId, Guid userId)
        {
            var existingFav = await _appDBContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CarId == carId);

            if(existingFav!=null)
            {
                _appDBContext.Favorites.Remove(existingFav);
                await _appDBContext.SaveChangesAsync();
            }
        }

        public async Task<List<Car>> GetFavoritesCars(Guid userId)
        {
            return await _appDBContext.Favorites
                .Where(f=>f.UserId==userId)
                .Select(f=>f.Car).ToListAsync();
        }

    }
}
