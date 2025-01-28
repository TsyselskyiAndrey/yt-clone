using System.ComponentModel.DataAnnotations;
using VibeoServer.DTOs.CustomValidations;

namespace VibeoServer.DTOs
{
    public class LogInModel
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

    }
}
