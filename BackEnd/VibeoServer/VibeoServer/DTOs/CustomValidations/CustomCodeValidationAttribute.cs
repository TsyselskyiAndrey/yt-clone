using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VibeoServer.DTOs.CustomValidations
{
    public class CustomCodeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string code = value as string;
            if (code == null)
            {
                return new ValidationResult("Code is required.");
            }
            if (code.Length != 6)
            {
                return new ValidationResult("The code is not complete.");
            }
            if(Regex.IsMatch(code, @"[^0-9]"))
            {
                return new ValidationResult("The code must only contain numbers.");
            }
            return ValidationResult.Success;
        }
    }
}
