using System.ComponentModel.DataAnnotations;
using VibeoServer.DTOs.CustomValidations;

namespace VibeoServer.DTOs
{
    public class RegisterStep2Model
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The email address you entered is invalid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [CustomNameValidation(
            MaximumLength = 20,
            MinimumLength = 2,
            AllowSpaces = false,
            AllowSymbols = false,
            AllowNums = false
        )]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [CustomSurnameValidation(
            MaximumLength = 20,
            MinimumLength = 2,
            AllowSpaces = false,
            AllowSymbols = false,
            AllowNums = false
        )]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Birthdate is required.")]
        [CustomDateValidation]
        public string BirthDate { get; set; }
    }
}
