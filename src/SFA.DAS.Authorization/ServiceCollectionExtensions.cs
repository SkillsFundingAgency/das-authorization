using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Cache;

namespace SFA.DAS.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization<DefaultAuthorizationContextProvider>();
        }
        
        public static IServiceCollection AddAuthorization<T>(this IServiceCollection services) where T : class, IAuthorizationContextProvider
        {
            return services.AddScoped<IAuthorizationContext, AuthorizationContext>()
                .AddScoped<IAuthorizationContextProvider>(p => new AuthorizationContextCache(p.GetService<T>()))
                .AddScoped<IAuthorizationService, AuthorizationService>()
                //TODO: add all implementations of IAuthorizationContextCacheKeyProvider by assembly scanning
                .AddScoped<T>()
                .AddSingleton<IAuthorizationCacheService, AuthorizationCacheService>();
        }
    }
}