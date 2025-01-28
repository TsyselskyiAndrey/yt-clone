using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VibeoServer.DTOs.CustomValidations
{
    public class CustomSurnameValidationAttribute : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 2;
        public int MaximumLength { get; set; } = 20;
        public bool AllowSpaces { get; set; } = false;
        public bool AllowSymbols { get; set; } = false;
        public bool AllowNums { get; set; } = false;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var name = value as string;
            if (name == null) return new ValidationResult("Surname is required.");
            if (name.Length < MinimumLength || name.Length > MaximumLength)
            {
                return new ValidationResult($"Surname must be between {MinimumLength} and {MaximumLength} characters.");
            }

            if (!AllowNums && Regex.IsMatch(name, @"\d"))
            {
                return new ValidationResult("Surname cannot contain numbers.");
            }

            if (!AllowSpaces && name.Contains(" "))
            {
                return new ValidationResult("Surname cannot contain spaces.");
            }

            if (!AllowSymbols && Regex.IsMatch(name, @"[^a-zA-Z0-9 ]"))
            {
                return new ValidationResult("Surname cannot contain symbols.");
            }

            return ValidationResult.Success;
        }
    }
}
