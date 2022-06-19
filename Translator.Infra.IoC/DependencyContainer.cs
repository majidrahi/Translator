using Translator.Application.Interfaces;
using Translator.Application.Services;
using Translator.Data.Repositories;
using Translator.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Translator.Infra.IoC
{
    public static class DependencyContainer
    {
        /// <summary>
        /// This extension method register DI services in Program.cs
        /// </summary>
        /// <param name="services"></param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ILeetTranslationService,LeetTranslationService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
    }
}