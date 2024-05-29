using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.ProviderPermissions.Client;

namespace SFA.DAS.Authorization.ProviderPermissions.DependencyResolution.Microsoft
{
    internal static class HttpServiceCollectionExtensions
    {
        public static IServiceCollection AddHttp(this IServiceCollection services)
        {
            return services
                .AddSingleton(p => p.GetRequiredService<IProviderRelationshipsApiClientFactory>().CreateClient())
                .AddTransient<IProviderRelationshipsApiClientFactory, ProviderRelationshipsApiClientFactory>();
        }
    }
}