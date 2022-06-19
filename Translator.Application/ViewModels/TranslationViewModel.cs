using Translator.Common.Enums;
using Translator.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Translator.Application.ViewModels
{
    public class TranslationCreateViewModel
    {
        [Required(ErrorMessage ="Please enter {0}")]
        public string SourceText { get; set; } = default!;
        public string? TranslatedText { get; set; }
        [Required]
        public bool IsSucceded { get; set; }
        [Required]
        public DateTimeOffset CreatedDateTime { get; set; }
        [Required]
        public TranslationType TranslationType { get; set; }
        [Required]
        public ApplicationUser User { get; set; } = default!;
    }
    public class TranslationViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Source Text")]
        public string SourceText { get; set; } = default!;
        [Display(Name ="Result")]
        public string? TranslatedText { get; set; }
        [Display(Name = "Status")]
        public bool IsSucceded { get; set; }
        [Display(Name = "Translation Type")]
        public string TranslationType { get; set; } = default!;
        [Display(Name = "Request Time")]
        public DateTimeOffset CreatedDateTime { get; set; }
    }

    public class TranslationInputViewModel
    {
        [Required(ErrorMessage ="Please enter {0}")]
        [Display(Name ="Source Text")]
        public string SourceText { get; set; } = default!;
    }

    public record struct TranslationResult(string? text, bool isSucceded);
}
