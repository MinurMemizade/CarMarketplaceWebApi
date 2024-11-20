namespace CarMarketplaceWebApi.Models.Security
{
    public class TokenSettings
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int TokenValidityInMinutes { get; set; }
    }
}
