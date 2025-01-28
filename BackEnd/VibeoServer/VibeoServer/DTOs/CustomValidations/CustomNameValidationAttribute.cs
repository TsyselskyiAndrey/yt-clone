using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VibeoServer.DTOs.CustomValidations
{
    public class CustomNameValidationAttribute : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 2;
        public int MaximumLength { get; set; } = 20;
        public bool AllowSpaces { get; set; } = false;
        public bool AllowSymbols { get; set; } = false;
        public bool AllowNums { get; set; } = false;
        public string FieldName { get; set; } = "Name";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var name = value as string;
            if (name == null) return new ValidationResult($"{FieldName} is required.");
            if (name.Length < MinimumLength || name.Length > MaximumLength)
            {
                return new ValidationResult($"{FieldName} must be between {MinimumLength} and {MaximumLength} characters.");
            }

            if (!AllowNums && Regex.IsMatch(name, @"\d"))
            {
                return new ValidationResult($"{FieldName} cannot contain numbers.");
            }

            if (!AllowSpaces && name.Contains(" "))
            {
                return new ValidationResult($"{FieldName} cannot contain spaces. Please remove any spaces and try again.");
            }

            if (!AllowSymbols && Regex.IsMatch(name, @"[^a-zA-Z0-9 ]"))
            {
                return new ValidationResult($"{FieldName} cannot contain symbols.");
            }

            return ValidationResult.Success;
        }
    }
}
