using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VibeoServer.DTOs.CustomValidations
{
    public class CustomDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string date = value as string;
            if (date == null) {
                return new ValidationResult("Date is required.");
            }
            DateTime parsedDate;
            if(!DateTime.TryParseExact(date, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
            {
                return new ValidationResult("The date is not valid.");
            }

            if (parsedDate.Year > DateTime.Now.Year)
            {
                return new ValidationResult("Are you from the future?");
            }
            if (parsedDate.Year + 13 > DateTime.Now.Year)
            {
                return new ValidationResult("You are too young.");
            }
            if (parsedDate.Year + 140 < DateTime.Now.Year)
            {
                return new ValidationResult("You are too old.");
            }
            return ValidationResult.Success;
        }
    }
}
