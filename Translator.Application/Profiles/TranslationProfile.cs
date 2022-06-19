
using AutoMapper;
using Translator.Application.ViewModels;
using Translator.Domain.Models;

namespace Translator.Application.Profiles
{
    /// <summary>
    /// Automapper profile map configuration
    /// </summary>
    public class TranslationProfile : Profile
    {
        public TranslationProfile()
        {
            CreateMap<TranslationCreateViewModel, Translation>().ReverseMap();
            CreateMap<Translation, TranslationViewModel>()
              .ForMember(x => x.CreatedDateTime, dto => dto.MapFrom(prop => prop.CreatedDateTime.ToLocalTime()));
        }
    }
}
