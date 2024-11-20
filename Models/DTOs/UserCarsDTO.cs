using CarMarketplaceWebApi.Models.Enums;

namespace CarMarketplaceWebApi.Models.DTOs
{
    public class UserCarsDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public EStatus eStatus { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Mileage { get; set; }
        public double Price { get; set; }
        public string Class { get; set; }
        public List<string> ImageUrls { get; set; }
        public bool IsNew { get; set; }
        public string SellerEmail { get; set; }
        public string SellerName { get; set; }
        public string Year { get; set; }
        public DateTime createdTime { get; set; }
        public DateTime? soldTime { get; set; }
    }
}
