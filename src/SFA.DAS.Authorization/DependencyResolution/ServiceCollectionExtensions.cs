using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Logging;
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
            return services.AddLogging()
                .AddMemoryCache()
                .AddScoped<IAuthorizationContextProvider>(p => new AuthorizationContextCache(p.GetService<T>()))
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<T>()
                .AddScoped(p => p.GetService<IAuthorizationContextProvider>().GetAuthorizationContext());
        }

        public static IServiceCollection AddAuthorizationHandler<T>(this IServiceCollection services, bool enableAuthorizationResultCache = false) where T : class, IAuthorizationHandler
        {
            return services.AddScoped<T>()
                .AddScoped(p =>
                {
                    var authorizationHandler = (IAuthorizationHandler)p.GetService(typeof(T));
                    var authorizationResultCacheConfigurationProviders = p.GetServices<IAuthorizationResultCacheConfigurationProvider>();
                    var memoryCache = p.GetService<IMemoryCache>();
                    var logger = p.GetService<ILogger<AuthorizationResultLogger>>();
                    
                    if (enableAuthorizationResultCache)
                    {
                        authorizationHandler = new AuthorizationResultCache(authorizationHandler, authorizationResultCacheConfigurationProviders, memoryCache);
                    }
                    
                    authorizationHandler = new AuthorizationResultLogger(authorizationHandler, logger);

                    return authorizationHandler;
                });
        }
    }
}