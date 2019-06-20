using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Startup
{
    public static class AuthenticationStartup
    {
        public static IServiceCollection AddDasAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = new PathString("/account/http401"));

            return services;
        }
    }
}