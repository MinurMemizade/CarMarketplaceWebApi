using CarMarketplaceWebApi.Models.DTOs;

namespace CarMarketplaceWebApi.Services.Interfaces
{
    public interface IRevokeService
    {
        Task Revoke(RevokeDTO revokeDTO);
        Task RevokeAll();
    }
}
