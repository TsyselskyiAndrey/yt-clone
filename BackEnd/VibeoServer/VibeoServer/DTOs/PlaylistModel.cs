using System.ComponentModel.DataAnnotations;
using VibeoServer.DTOs.CustomValidations;

namespace VibeoServer.DTOs
{
    public class PlaylistModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [CustomNameValidation(
            MaximumLength = 255,
            MinimumLength = 2,
            AllowSpaces = true,
            AllowSymbols = true,
            AllowNums = true,
            FieldName = "Title"
        )]
        public string Title { get; set; }

        [CustomNameValidation(
            MaximumLength = 1000,
            MinimumLength = 0,
            AllowSpaces = true,
            AllowSymbols = true,
            AllowNums = true,
            FieldName = "Description"
        )]
        public string? Description { get; set; }
    }
}
