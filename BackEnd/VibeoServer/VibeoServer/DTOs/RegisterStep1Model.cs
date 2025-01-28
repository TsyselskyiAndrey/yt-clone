using System.ComponentModel.DataAnnotations;
using VibeoServer.DTOs.CustomValidations;

namespace VibeoServer.DTOs
{
    public class RegisterStep1Model
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The email address you entered is invalid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [CustomPasswordValidation(
            MinimumLength = 12,
            MaximumLength = 30,
            RequireNumbers = true,
            RequireBothCases = true,
            AllowSpaces = false,
            AllowSymbols = false
        )]
        public string Password { get; set; }

        [Required(ErrorMessage = "Re-password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string RePassword { get; set; }
    }
}
