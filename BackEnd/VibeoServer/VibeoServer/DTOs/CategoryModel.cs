using System.ComponentModel.DataAnnotations;
using VibeoServer.DTOs.CustomValidations;

namespace VibeoServer.DTOs
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "Choose a category.")]
        [CustomNameValidation(
            MaximumLength = 20,
            MinimumLength = 2,
            AllowSpaces = true,
            AllowSymbols = false,
            AllowNums = false,
            FieldName = "Category"
        )]
        public string Name { get; set; }
    }
}
