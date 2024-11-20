using CarMarketplaceWebApi.Models.Common;
using System.Text.Json.Serialization;

namespace CarMarketplaceWebApi.Models.DTOs
{
    public class CarDTO
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public string Year { get; set; }
        public string Class { get; set; }
        public string Color { get; set; }
        public string Mileage { get; set; }
        public bool IsNew { get; set; }
        public IEnumerable<IFormFile> Images { get; set; }
    }

    public class UpdateCarDTO
    {
        public Guid id { get; set; }   
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public string Year { get; set; }
        public string Class { get; set; }
        public string Color { get; set; }
        public string Mileage { get; set; }
        public bool IsNew { get; set; }
        public IEnumerable<IFormFile> Images { get; set; }
    }
}
