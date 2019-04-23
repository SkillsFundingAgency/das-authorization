using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationContext, AuthorizationContext>()
                .AddScoped<IAuthorizationService, AuthorizationService>();
        }
    }
}