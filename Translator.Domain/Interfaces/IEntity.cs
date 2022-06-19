using System.ComponentModel.DataAnnotations;

namespace Translator.Domain.Interfaces
{
    /// <summary>
    /// An interface take advantage of Generic stuff
    /// </summary>
    public interface IEntity
    {
        [Key]
        int Id { get; set; }
        DateTimeOffset CreatedDateTime { get; set; }

    }
}
