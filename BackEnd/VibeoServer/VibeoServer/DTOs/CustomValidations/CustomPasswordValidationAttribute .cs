using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VibeoServer.DTOs.CustomValidations
{
    public class CustomPasswordValidationAttribute : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 12;
        public int MaximumLength { get; set; } = 30;
        public bool RequireNumbers { get; set; } = true;
        public bool RequireBothCases { get; set; } = true;
        public bool AllowSpaces { get; set; } = false;
        public bool AllowSymbols { get; set; } = false;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;
            if (password == null) return new ValidationResult("Password is required.");
            if (password.Length < MinimumLength || password.Length > MaximumLength)
            {
                return new ValidationResult($"Password must be between {MinimumLength} and {MaximumLength} characters.");
            }

            if (RequireNumbers && !Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult("Password must contain at least one number.");
            }

            if (RequireBothCases && !Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])"))
            {
                return new ValidationResult("Password must contain at least one uppercase and one lowercase letter.");
            }

            if (!AllowSpaces && password.Contains(" "))
            {
                return new ValidationResult("Password cannot contain spaces.");
            }

            if (!AllowSymbols && Regex.IsMatch(password, @"[^a-zA-Z0-9 ]"))
            {
                return new ValidationResult("Password cannot contain symbols.");
            }

            return ValidationResult.Success;
        }
    }
}
