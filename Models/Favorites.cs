using CarMarketplaceWebApi.Models.Identity;

namespace CarMarketplaceWebApi.Models
{
    public class Favorites
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CarId { get; set; }
        public virtual Car Car { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
