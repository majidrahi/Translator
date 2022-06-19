using Translator.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Translator.Data.Context
{
    public class TranslatorDbContext: IdentityDbContext
    {
        public TranslatorDbContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<Translation> Translations { get; set; } = null!;
    }
}
