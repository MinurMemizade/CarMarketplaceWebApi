using CarMarketplaceWebApi.Models.DTOs;
using CarMarketplaceWebApi.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarMarketplaceWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IRevokeService revokeService;

        public AuthController(IAuthService authService, IRevokeService revokeService)
        {
            this.authService = authService;
            this.revokeService = revokeService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            await authService.RegisterAsync(registerDTO);
            return StatusCode(StatusCodes.Status201Created,"Verification token has sent to your email.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            return Ok(await authService.LoginAsync(loginDTO));
        }

        [HttpPost("Revoke")]
        public async Task<IActionResult> Revoke([FromForm] RevokeDTO revokeDTO)
        {
            revokeService.Revoke(revokeDTO);
            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpPost("RevokeAll")]
        public async Task<IActionResult> RevokeAll()
        {
            await revokeService.RevokeAll();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromForm] VerificationDTO verificationDTO)
        {
            await authService.VerifyEmail(verificationDTO);
            return StatusCode(StatusCodes.Status202Accepted,"Email confirmed.");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordDTO forgotPasswordDTO)
        {
            await authService.ForgotPassword(forgotPasswordDTO);
            return Ok("Password reset key has been sent.");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDTO resetPasswordDTO)
        {
            await authService.ResetPassword(resetPasswordDTO);
            return Ok("Password changed successfully.");
        }

    }
}
