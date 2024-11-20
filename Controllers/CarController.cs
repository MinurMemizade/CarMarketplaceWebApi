using CarMarketplaceWebApi.Models;
using CarMarketplaceWebApi.Models.DTOs;
using CarMarketplaceWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarMarketplaceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        
        [HttpGet("GetAllCars")]
        public async Task<IActionResult> GetAllCars()
        {
            var result = await _carService.GetAllAsync();
            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetUsersCars")]
        public async Task<IActionResult> GetUsersCars()
        {
            var result=await _carService.GetAllCurrrentUserCars();
            return StatusCode(StatusCodes.Status200OK,result);
        }

        
        [HttpGet("GetCarsById")]
        public async Task<IActionResult> GetCarsById(Guid id)
        {
            var result= await _carService.GetByIdAsync(id);
            return StatusCode(StatusCodes.Status200OK,result);
        }

        [HttpGet("GetFavoriteCars")]
        public async Task<IActionResult> GetFavoiteCars()
        {
            var result = await _carService.GetFavoritesCars();
            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost("AddToFavorites")]
        public async Task<IActionResult> AddToFavorites(Guid carId)
        {
            await _carService.AddFavorites(carId);
            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpDelete("RemoveFromFavorites")]
        public async Task<IActionResult> RemoveFromFavorites(Guid carId)
        {
            await _carService.RemoveFromFavorites(carId);
            return StatusCode(StatusCodes.Status202Accepted);
        }


        [HttpPost("AddCar")]
        public async Task<IActionResult> AddCar([FromForm] CarDTO carDTO)
        {
            await _carService.CreateAsync(carDTO);
            return StatusCode(StatusCodes.Status201Created, carDTO);
        }

        [Authorize]
        [HttpPut("UpdateCar")]
        public async Task<IActionResult> UpdateCar([FromForm] UpdateCarDTO updateCarDTO)
        {
            await _carService.UpdateAsync(updateCarDTO);
            return StatusCode(StatusCodes.Status202Accepted, updateCarDTO);
        }

        [Authorize]
        [HttpDelete("DeleteCar")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            await _carService.DeleteAsync(id);
            return StatusCode(StatusCodes.Status202Accepted,null);
        }

        [HttpPost("Approve")]
        public async Task<IActionResult> ApproveCarRequest([FromForm] Guid id,[FromForm] string status)
        {
            await _carService.ApproveRequest(id,status);
            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpGet("GetApprovedCars")]
        public async Task<IActionResult> GetApprovedCars()
        {
            var result=await _carService.GetApprovedCars();
            return Ok(result);
        }

        [HttpGet("GetPendingCars")]
        public async Task<IActionResult> GetPendingCars()
        {
            var result =await _carService.GetPendingCars();
            return Ok(result);
        }

        [HttpPost("ComplainCar")]
        public async Task<IActionResult> ComplainCar([FromForm] ComplainDTO complainDTO)
        {
            await _carService.ComplainCar(complainDTO);
            return StatusCode(StatusCodes.Status202Accepted);
        }
    }
}
