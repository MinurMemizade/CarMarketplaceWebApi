using CarMarketplaceWebApi.Models.DTOs;
using CarMarketplaceWebApi.Models.Identity;
using CarMarketplaceWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarMarketplaceWebApi.Services.Implementations
{
    public class RevokeService : IRevokeService
    {
        private readonly UserManager<AppUser> _userManager;

        public RevokeService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Revoke(RevokeDTO revokeDTO)
        {
            AppUser user = await _userManager.FindByEmailAsync(revokeDTO.Email);
            if (user == null) throw new Exception("There is not such a user.");
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (AppUser user in users)
            {
                if (user.RefreshToken != null)
                {
                    user.RefreshToken = null;
                    await _userManager.UpdateAsync(user);
                }
            }
        }
    }
}
