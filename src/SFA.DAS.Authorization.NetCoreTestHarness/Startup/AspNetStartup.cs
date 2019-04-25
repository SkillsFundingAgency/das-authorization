using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.Authorization.NetCoreTestHarness.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Startup
{
    public class AspNetStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = new PathString("/account/http401"));

            services.AddEmployerFeaturesAuthorization();

            services.AddMvc(o => o.AddAuthorization())
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry);
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage()
                .UseUnauthorizedAccessExceptionHandler()
                .UseStatusCodePagesWithReExecute("/account/http{0}")
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseAuthentication()
                .UseMvc(r =>
                {
                    r.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}