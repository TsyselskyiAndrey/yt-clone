using System.ComponentModel.DataAnnotations;
using VibeoServer.DTOs.CustomValidations;

namespace VibeoServer.DTOs
{
    public class RegisterStep3Model
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The email address you entered is invalid.")]
        public string Email { get; set; }

        [Required]
        [CustomCodeValidation]
        public string Code { get; set; }
    }
}
