using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.ProviderPermissions.Handlers;
using SFA.DAS.AutoConfiguration.DependencyResolution;

namespace SFA.DAS.Authorization.ProviderPermissions.DependencyResolution.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderPermissionsAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorizationHandler<AuthorizationHandler>()
                .AddAutoConfiguration()
                .AddHttp();
        }
    }
}