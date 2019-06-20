using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Services;

namespace SFA.DAS.Authorization.DependencyResolution
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
                .AddScoped<T>();
        }
    }
}