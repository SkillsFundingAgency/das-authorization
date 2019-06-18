using Microsoft.Extensions.DependencyInjection;

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
                .AddScoped<T>();
        }
    }
}