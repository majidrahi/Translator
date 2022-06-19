using AutoMapper;
using Translator.Application.Interfaces;
using Translator.Application.ViewModels;
using Translator.Domain.Interfaces;
using Translator.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Translator.Application.Services
{
    public class LeetTranslationService : ILeetTranslationService
    {
        private readonly IRepository<Translation> _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public LeetTranslationService(IRepository<Translation> repository, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager,
IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this._repository = repository;
            this._httpClientFactory = httpClientFactory;
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
        }
        public async Task AddTranslation(TranslationCreateViewModel t)
        {
            var model = _mapper.Map<Translation>(t);
            await _repository.Add(model);
            await _repository.Save();
        }

        public async Task<IEnumerable<TranslationViewModel>> GetAll()
        {
            var model = await _repository.GetAll();
            return _mapper.Map<List<TranslationViewModel>>(model);
        }

        public async Task<int> GetAllCount()
        {
            return await _repository.GetAllCount();
        }

        public async Task<IReadOnlyCollection<TranslationViewModel>> GetByFilter(Expression<Func<TranslationViewModel, bool>>? filter, Expression<Func<IQueryable<TranslationViewModel>, IOrderedQueryable<TranslationViewModel>>>? orderBy = null,
            string includeProperties = "",
            int first = 0, int offset = 0)
        {
            var modelFilter= _mapper.Map<Expression<Func<Translation, bool>>>(filter);
            var mappedOrderBy = _mapper.Map<Expression<Func<IQueryable<Translation>, IOrderedQueryable<Translation>>>>(orderBy);
            Func<IQueryable<Translation>, IOrderedQueryable<Translation>> modelOrderBy = query => query.OrderBy(x => x.Id);
            if (mappedOrderBy !=null)
            {
                 modelOrderBy = mappedOrderBy.Compile();
            }

            var model = await _repository.GetByFilter(modelFilter, modelOrderBy, includeProperties, first, offset);

            return _mapper.Map<IReadOnlyCollection<TranslationViewModel>>(model);
        }


        public async Task<int> GetCountByFilter(Expression<Func<Translation, bool>> filter)
        {
            return await _repository.GetCountByFilter(filter);
        }

        public async Task<TranslationResult> Translate(string sourceText)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return new TranslationResult(null,false);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new TranslationResult(null, false);
            }


            var httpClient = _httpClientFactory.CreateClient("Translate");
            var content = new FormUrlEncodedContent(new[]
                        {
                new KeyValuePair<string, string>("text", sourceText)
            });
            var result = await httpClient.PostAsync("/translate/leetspeak.json", content);
            var translatedResult = await result.Content.ReadAsStringAsync();
            var translatedObj = JsonConvert.DeserializeObject<dynamic>(translatedResult);
            string? translatedText = null;
            var successStatus = false;
            if (translatedObj?.contents != null && translatedObj?.contents.translated != null)
            {
                translatedText = translatedObj?.contents.translated;
                successStatus = true;
            }
            else if (translatedObj?.error != null)
            {
                translatedText = translatedObj?.error.message;
            }

            await AddTranslation(new TranslationCreateViewModel()
            {
                SourceText = sourceText,
                TranslatedText = translatedText,
                CreatedDateTime = DateTimeOffset.UtcNow,
                IsSucceded = successStatus,
                User = user
            });

            return new TranslationResult(translatedText, successStatus);
        }


    }
}
