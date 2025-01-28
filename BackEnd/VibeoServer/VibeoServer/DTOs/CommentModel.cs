using System.ComponentModel.DataAnnotations;
using VibeoServer.DTOs.CustomValidations;

namespace VibeoServer.DTOs
{
    public class CommentModel
    {
        [Required(ErrorMessage = "Type something first.")]
        [CustomNameValidation(
            MaximumLength = 1000,
            MinimumLength = 1,
            AllowSpaces = true,
            AllowSymbols = true,
            AllowNums = true,
            FieldName = "Comment"
        )]
        public string Message { get; set; }
    }
}
