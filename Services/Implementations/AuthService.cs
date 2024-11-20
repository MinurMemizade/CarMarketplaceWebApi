using CarMarketplaceWebApi.Exceptions;
using CarMarketplaceWebApi.Helpers;
using CarMarketplaceWebApi.Models.DTOs;
using CarMarketplaceWebApi.Models.Identity;
using CarMarketplaceWebApi.Services.Interfaces;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace CarMarketplaceWebApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly TokenService _tokenService;
        private readonly IFluentEmail fluentEmail;

        public AuthService(UserManager<AppUser> userManager, TokenService tokenService, IConfiguration configuration, RoleManager<Role> roleManager, IFluentEmail fluentEmail)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _roleManager = roleManager;
            this.fluentEmail = fluentEmail;
        }

        public async Task<ResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var oldUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (oldUser == null) throw new Exception("Email or Password is incorrect.");

            if(!await _userManager.IsEmailConfirmedAsync(oldUser))
            {
                await SendVerificationEmailAsync(oldUser);  
                throw new Exception("Email is not verified");
            }

            if (!await _userManager.CheckPasswordAsync(oldUser, loginDTO.Password)) throw new Exception("Email or Password is incorrect.");


            var userRole = await _userManager.GetRolesAsync(oldUser);
            var jwtToken = await _tokenService.CreateToken(oldUser, userRole);
            string refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            oldUser.RefreshToken = refreshToken;
            oldUser.RefreshTokenExpireTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(oldUser);
            await _userManager.UpdateSecurityStampAsync(oldUser);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(jwtToken);

            await _userManager.SetAuthenticationTokenAsync(oldUser, "Default", "AccessToken", tokenString);


            return new ResponseDTO
            {
                StatusCode = 200,
                JWTToken = tokenString,
                RefreshToken = refreshToken,
                Expiration = jwtToken.ValidTo
            };

        }

        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            var isExist=await _userManager.FindByEmailAsync(registerDTO.Email);
            if (isExist !=null) throw new UserAlreadyExistsException();
            
            var newUser = new AppUser()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("admin"))
                {
                    await _roleManager.CreateAsync(new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    });
                    await _userManager.AddToRoleAsync(newUser, "admin");
                }

               await SendVerificationEmailAsync(newUser);

            }
        }

        public async Task ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user=await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user == null) 
            {
                throw new Exception("User not found.");
            }

            var token=await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await fluentEmail
                .To(user.Email)
                .Subject("Email Verification")
                .Body("Your password refresh token:" + "\n" + token)
                .SendAsync();

            if (!result.Successful) throw new Exception("Failed to send verification email.");
        }

        public async Task ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                throw new Exception("User not found.");
            };

            await _userManager.ResetPasswordAsync(user,resetPasswordDTO.PasswordResetToken, resetPasswordDTO.Password);

        }

        public async Task SendVerificationEmailAsync(AppUser newUser)
        {
            if (newUser == null) throw new ArgumentNullException("User is not found.");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            
            var result=await fluentEmail
                .To(newUser.Email)
                .Subject("Email Verification")
                .Body("Your email verification code:" + "\n" + code)
                .SendAsync();

            if (!result.Successful) throw new Exception("Failed to send verification email.");
        }


        public async Task VerifyEmail(VerificationDTO verificationDTO)
        {
            var user = await _userManager.FindByEmailAsync(verificationDTO.Email);
            if (user == null) throw new Exception("User not found.");

            await _userManager.ConfirmEmailAsync(user, verificationDTO.VerificationToken);
        }
    }
}
