using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VibeoServer.DTOs;
using VibeoServer.Helpers;
using VibeoServer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authentication.Cookies;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using VibeoServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;

namespace VibeoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly int codeDuration = 5;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly BlobServiceClient _blobServiceClient;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJWTService jwtService, RoleManager<IdentityRole> roleManager, IEmailService emailService, BlobServiceClient blobServiceClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _emailService = emailService;
            _blobServiceClient = blobServiceClient;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .SelectMany(m => m.Value.Errors.Select(e => new CustomResponse
                    {
                        Field = m.Key,
                        Type = "ValidationError",
                        Details = e.ErrorMessage
                    }))
                    .ToList();

                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }
            var user = await _userManager.Users
                .Include(u => u.Channel)
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new List<CustomResponse> { 
                    new() { 
                        Type="ValidationError", 
                        Field = "Email", 
                        Details = "Incorrect email! There is no user with this email." 
                    } 
                });
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {
                var accessToken = await _jwtService.GenerateAccessToken(user);
                (string refreshToken, int refreshTokenValidityInDays) = _jwtService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.Now.AddDays(refreshTokenValidityInDays)
                    };
                    Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                    return Ok(new { 
                        AccessToken = accessToken,
                        User = new
                        {
                            user.FirstName,
                            user.LastName,
                            user.Email,
                            user.AvatarUrl,
                            user.BirthDate,
                            user.Channel?.Handle,
                            user.Channel?.Title,
                            ChannelId = user.Channel?.Id,
                            user.Channel?.LogoUrl,
                            Roles = await _userManager.GetRolesAsync(user)
                        }
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new List<CustomResponse> {
                new() {
                    Type="ValidationError",
                    Field = "Password",
                    Details = "Incorrect password."
                }
            });
        }

        [HttpPost("google-response")]
        public async Task<IActionResult> GoogleResponse([FromBody] GoogleModel googleResponse)
        {
            if(googleResponse == null)
            {
                return BadRequest();
            }

            var userEmail = googleResponse.Email;
            var user = await _userManager.Users
                .Include(u => u.Channel)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                var newUser = new User()
                {
                    Email = googleResponse.Email,
                    UserName = googleResponse.Email,
                    FirstName = googleResponse.Given_name,
                    LastName = googleResponse.Family_name,
                    AvatarUrl = "https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/default.png",
                    CreatedAt = DateTime.Now,
                    EmailConfirmed = googleResponse.Email_verified
                };
                var result = await _userManager.CreateAsync(newUser);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create user.");
                }
                else
                {
                    user = newUser;
                }
            }

            if (!user.EmailConfirmed)
            {
                user.Email = googleResponse.Email;
                user.UserName = googleResponse.Email;
                user.FirstName = googleResponse.Given_name;
                user.LastName = googleResponse.Family_name;
                user.AvatarUrl = "https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/default.png";
                user.CreatedAt = DateTime.Now;
                user.EmailConfirmed = googleResponse.Email_verified;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update user.");
                }
            }

            var accessToken = await _jwtService.GenerateAccessToken(user);
            (string refreshToken, int refreshTokenValidityInDays) = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddDays(refreshTokenValidityInDays)
                };
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                return Ok(new { 
                    AccessToken = accessToken,
                    User = new
                    {
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.AvatarUrl,
                        user.BirthDate,
                        user.Channel?.Handle,
                        user.Channel?.Title,
                        ChannelId = user.Channel?.Id,
                        user.Channel?.LogoUrl,
                        Roles = await _userManager.GetRolesAsync(user)
                    }
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPost("register-step1")]
        public async Task<IActionResult> RegisterStep1([FromBody] RegisterStep1Model model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .SelectMany(m => m.Value.Errors.Select(e => new CustomResponse
                    {
                        Field = m.Key,
                        Type = "ValidationError",
                        Details = e.ErrorMessage
                    }))
                    .ToList();

                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                if (userExists.EmailConfirmed)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new List<CustomResponse> {
                        new() {
                            Type="ValidationError",
                            Field = "Email",
                            Details = "Such a user already exists!"
                        }
                    });
                }
                userExists.Email = model.Email;
                userExists.UserName = model.Email;
                userExists.CreatedAt = DateTime.Now;
                
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(userExists);
                var setPasswordResult = await _userManager.ResetPasswordAsync(userExists, passwordResetToken, model.Password);
                if (!setPasswordResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to reset password.");
                }

                var updateResult = await _userManager.UpdateAsync(userExists);
                if (!updateResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update user information.");
                }

                return Ok("The user has been updated succefully!");
            }
                

            var user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok("The user has been created succefully!");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.");
            }

        }

        [HttpPost("register-step2")]
        public async Task<IActionResult> RegisterStep2([FromBody] RegisterStep2Model model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .SelectMany(m => m.Value.Errors.Select(e => new CustomResponse
                    {
                        Field = m.Key,
                        Type = "ValidationError",
                        Details = e.ErrorMessage
                    }))
                    .ToList();

                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Something went wrong! Reload the page and try again.");
            }
            if (user.EmailConfirmed)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Something went wrong! Reload the page and try again.");
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.BirthDate = DateTime.ParseExact(model.BirthDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            string code = GenerateConfirmationCode();
            var expirationDate = DateTime.Now.AddMinutes(codeDuration);

            user.EmailConfirmationCode = code;
            user.EmailConfirmationCodeExpiryTime = expirationDate;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await SendConfirmationCodeByEmail(user.Email, code);
                return Ok(new { EmailConfirmationCodeExpiryTime = expirationDate });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "User updating failed! Please check user details and try again.");

        }


        [HttpPost("register-step3")]
        public async Task<IActionResult> RegisterStep3([FromBody] RegisterStep3Model model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                     .Where(m => m.Value.Errors.Any())
                     .SelectMany(m => m.Value.Errors.Select(e => new CustomResponse
                     {
                         Field = m.Key,
                         Type = "ValidationError",
                         Details = e.ErrorMessage
                     }))
                     .ToList();

                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Something went wrong! Reload the page and try again.");
            }
            if (user.EmailConfirmed)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Something went wrong! Reload the page and try again.");
            }
            if (user.EmailConfirmationCodeExpiryTime <= DateTime.Now || user.EmailConfirmationCode == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new List<CustomResponse> {
                    new() {
                        Type="ValidationError",
                        Field = "Code",
                        Details = "The code is either incorrect or expired. Try again."
                    }
                });
            }

            if(model.Code == user.EmailConfirmationCode)
            {
                user.EmailConfirmed = true;
                user.EmailConfirmationCode = null;
                user.EmailConfirmationCodeExpiryTime = DateTime.MinValue;
                user.AvatarUrl = "https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/default.png";

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok("The email has been confirmed succefully!");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Email confirmation failed.");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new List<CustomResponse> {
                    new() {
                        Type="ValidationError",
                        Field = "Code",
                        Details = "The code is incorrect."
                    }
                });
            }
            
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }
            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = Request.Cookies["refreshToken"];

            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return Unauthorized("Invalid access token");
            }

            string? email = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return Unauthorized("Invalid access token");
            }

            var user = await _userManager.Users
                .Include(u => u.Channel)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized("Invalid access token or refresh token");
            }

            var newAccessToken = await _jwtService.GenerateAccessToken(user);
            (string newRefreshToken, int newRefreshTokenValidityInDays) = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(newRefreshTokenValidityInDays);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddDays(newRefreshTokenValidityInDays)
                };
                Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

                return Ok(new { 
                    AccessToken = newAccessToken,
                    User = new
                    {
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.AvatarUrl,
                        user.BirthDate,
                        user.Channel?.Handle,
                        user.Channel?.Title,
                        ChannelId = user.Channel?.Id,
                        user.Channel?.LogoUrl,
                        Roles = await _userManager.GetRolesAsync(user)
                    }    
                });
            }
            else
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while logging out.");
            }

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok("User logged out successfully.");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("haschannel")]
        public async Task<IActionResult> Haschannel()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                bool hasChannelRole = User.IsInRole("channel_owner");
                return Ok(hasChannelRole);
            }
            return Unauthorized("User is not authenticated.");
        }


        private string GenerateConfirmationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task SendConfirmationCodeByEmail(string email, string confirmationCode)
        {
            string subject = "Email Confirmation Code";

            string htmlContent = $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ccc;'>
                        <h2 style='color: #333;'>Confirm Your Email Address</h2>
                        <p style='font-size: 16px; color: #555;'>Thank you for registering! Please use the following code to complete your registration:</p>
                        <div style='padding: 15px; background-color: #f7f7f7; text-align: center; border-radius: 5px;'>
                            <h3 style='color: #444;'>{confirmationCode}</h3>
                        </div>
                        <p style='font-size: 16px; color: #555;'>If you didn't request this, please ignore this email.</p>
                        <p style='font-size: 14px; color: #aaa;'>Best regards,<br>Your Application Team</p>
                    </div>
                </body>
            </html>";

            await _emailService.SendEmailAsync(email, subject, htmlContent);
        }
    }
}
