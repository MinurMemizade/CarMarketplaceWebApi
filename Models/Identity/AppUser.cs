using Microsoft.AspNetCore.Identity;

namespace CarMarketplaceWebApi.Models.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public List<Favorites>? Favorites { get; set; }
        public List<Car>? UserCars { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireTime { get; set; }
    }
}
