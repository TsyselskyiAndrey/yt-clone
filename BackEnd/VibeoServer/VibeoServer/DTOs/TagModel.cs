using System.ComponentModel.DataAnnotations;
using VibeoServer.DTOs.CustomValidations;

namespace VibeoServer.DTOs
{
    public class TagModel
    {
        [Required(ErrorMessage = "Enter a tag.")]
        [CustomNameValidation(
            MaximumLength = 20,
            MinimumLength = 2,
            AllowSpaces = false,
            AllowSymbols = false,
            AllowNums = true,
            FieldName = "Tag"
        )]
        public string Name { get; set; }
    }
}
