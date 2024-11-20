using CarMarketplaceWebApi.Models.Common;
using CarMarketplaceWebApi.Models.Enums;
using CarMarketplaceWebApi.Models.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Eventing.Reader;

namespace CarMarketplaceWebApi.Models
{
    public class Car:BaseEntity
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public string Year { get; set; }
        public string Class { get; set; }
        public string Color { get; set; }
        public string Mileage { get; set; }
        public string SellerName { get; set; }
        public string SellerEmail { get; set; }
        public bool IsNew { get; set; }
        public EStatus eStatus { get; set; }
        public DateTime createdTime { get; set; }
        public DateTime? soldTime { get; set; }
        public List<string> ImageUrls { get; set; }
        public AppUser AppUser { get; set; }
        public Guid UserId { get; set; }
    }
}
