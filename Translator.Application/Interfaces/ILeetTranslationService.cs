using Translator.Application.ViewModels;
using Translator.Domain.Models;
using System.Linq.Expressions;

namespace Translator.Application.Interfaces
{
    public interface ILeetTranslationService
    {
        Task<TranslationResult> Translate(string sourceText);
        Task AddTranslation(TranslationCreateViewModel t);
        Task<IEnumerable<TranslationViewModel>> GetAll();
        Task<int> GetAllCount();
        Task<IReadOnlyCollection<TranslationViewModel>> GetByFilter(Expression<Func<TranslationViewModel, bool>> ?filter,
            Expression<Func<IQueryable<TranslationViewModel>, IOrderedQueryable<TranslationViewModel>>>? orderBy = null,
            string includeProperties = "",
            int first = 0, int offset = 0);
        Task<int> GetCountByFilter(Expression<Func<Translation, bool>> filter);
    }
}
