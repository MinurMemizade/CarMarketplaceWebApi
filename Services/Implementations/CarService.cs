using CarMarketplaceWebApi.Exceptions;
using CarMarketplaceWebApi.Models;
using CarMarketplaceWebApi.Models.DTOs;
using CarMarketplaceWebApi.Models.Enums;
using CarMarketplaceWebApi.Models.Identity;
using CarMarketplaceWebApi.Repositories.Interfaces;
using CarMarketplaceWebApi.Services.Interfaces;
using FluentEmail.Core;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Identity;

namespace CarMarketplaceWebApi.Services.Implementations
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IFileManagerService _fileManagerService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IFluentEmail _fluentEmail;

        public CarService(ICarRepository carRepository, IFileManagerService fileManagerService, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IConfiguration configuration, IFluentEmail fluentEmail)
        {
            _carRepository = carRepository;
            _fileManagerService = fileManagerService;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
            _fluentEmail = fluentEmail;
        }

        public async Task CreateAsync(CarDTO carDTO)
        {
            var imageUrls = await _fileManagerService.UploadFilesAsync(carDTO.Images);

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            if (user == null) throw new UserNotFoundException(); 

            Car car = new Car()
            {
                Make = carDTO.Make,
                Mileage = carDTO.Mileage,
                Model = carDTO.Model,
                Class = carDTO.Class,
                Color = carDTO.Color,
                IsNew = carDTO.IsNew,
                Price = carDTO.Price,
                Year = carDTO.Year,
                UserId=user.Id,
                eStatus=EStatus.PENDING,
                createdTime=DateTime.UtcNow,
                ImageUrls = imageUrls,
                SellerEmail=user.Email,
                SellerName=user.UserName
            };
            await _carRepository.CreateAsync(car);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _carRepository.DeleteAsync(id);
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        public async Task<Car> GetByIdAsync(Guid id)
        {
            return await _carRepository.GetAsync(id);
        }

        public async Task UpdateAsync(UpdateCarDTO updateCarDTO)
        {
            var imageUrls = await _fileManagerService.UploadFilesAsync(updateCarDTO.Images);
            var existingCar=await _carRepository.GetAsync(updateCarDTO.id);
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);

            if (user == null) throw new UserNotFoundException();

            if (existingCar == null) throw new CarNotFoundException();


            existingCar.Id = updateCarDTO.id;
            existingCar.Make = updateCarDTO.Make;
            existingCar.Mileage = updateCarDTO.Mileage;
            existingCar.Model = updateCarDTO.Model;
            existingCar.Class = updateCarDTO.Class;
            existingCar.Color = updateCarDTO.Color;
            existingCar.IsNew = updateCarDTO.IsNew;
            existingCar.Price = updateCarDTO.Price;
            existingCar.Year = updateCarDTO.Year;
            existingCar.SellerEmail = user.Email;
            existingCar.SellerName = user.UserName;
            existingCar.ImageUrls = imageUrls;
             
             await _carRepository.UpdateAsync(existingCar);
        }

        public async Task ApproveRequest(Guid id,string action)
        {
            var car =await _carRepository.GetAsync(id);
            if (car == null) throw new CarNotFoundException();

            if (action == "approve") car.eStatus = EStatus.ACTIVE;
            else if (action == "rejected") car.eStatus = EStatus.REJECTED;
            else throw new Exception("An error occured.");
            await _carRepository.UpdateAsync(car);
        }

        public async Task<IEnumerable<Car>> GetApprovedCars()
        {
            return await _carRepository.GetCarsByStatusAsync(1);
        }

        public async Task<IEnumerable<Car>> GetPendingCars()
        {
            return await _carRepository.GetCarsByStatusAsync(0);
        }

        public async Task<IEnumerable<UserCarsDTO>> GetAllCurrrentUserCars()
        {
            var user=await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            var cars = await _carRepository.GetCarsByUserId(user.Id);
            if(user==null) throw new UserNotFoundException();
            if(cars==null) throw new CarNotFoundException();
            return cars;
        }

        public async Task AddFavorites(Guid carId)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            if(user==null) throw new UserNotFoundException();
            await _carRepository.AddFavorite(carId, user.Id);
        }

        public async Task RemoveFromFavorites(Guid carId)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            if (user == null) throw new UserNotFoundException();
            await _carRepository.RemoveFromFavorite(carId, user.Id);
        }

        public async Task<List<Car>> GetFavoritesCars()
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            if (user == null) throw new UserNotFoundException();
            return await _carRepository.GetFavoritesCars(user.Id);
        }

        public async Task ComplainCar(ComplainDTO complainDTO)
        {
            var car=await _carRepository.GetAsync(complainDTO.CarId);
            if (car == null) throw new CarNotFoundException();

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            if (user == null) if (user == null) throw new UserNotFoundException();

            var adminEmail = "ndng.minurmemizade@gmail.com";
            var subject= $"Car Complaint for Car ID: {car.Id}";
            var body = $@"
                A complaint has been lodged for the following car:
                - Make: {car.Make}
                - Model: {car.Model}
                - Year: {car.Year}
                - Price: {car.Price:$}

                Complaint Details:
                {complainDTO.Complaint}

                Complainant Email: {user.Email ?? "Anonymous"}";

            var email = await _fluentEmail
                .To(adminEmail)
                .Subject(subject)
                .Body(body).SendAsync();
        }
    }
}
