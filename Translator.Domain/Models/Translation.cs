
using Translator.Common.Enums;
using Translator.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Translator.Domain.Models
{
    public class Translation :IEntity
    {
        [Key]
        public int Id { get; set; }
        public string SourceText { get; set; } = default!;
        public string? TranslatedText { get; set; } = default!;
        public DateTimeOffset CreatedDateTime { get; set; }
        public bool IsSucceded { get; set; }
        public TranslationType TranslationType { get; set; }
        public virtual ApplicationUser User { get; set; } = default!;
    }
}
