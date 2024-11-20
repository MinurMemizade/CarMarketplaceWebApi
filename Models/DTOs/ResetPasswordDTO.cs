using System.ComponentModel.DataAnnotations;

namespace CarMarketplaceWebApi.Models.DTOs
{
    public class ResetPasswordDTO
    {
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string PasswordResetToken { get; set; }
    }
}
