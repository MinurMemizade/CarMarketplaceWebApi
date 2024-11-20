using CarMarketplaceWebApi.Models.DTOs;

namespace CarMarketplaceWebApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO registerDTO);
        Task<ResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task VerifyEmail(VerificationDTO verifyDTO);
        Task ForgotPassword(ForgotPasswordDTO forgotPasswordDTO);
        Task ResetPassword(ResetPasswordDTO resetPasswordDTO);
    }
}
