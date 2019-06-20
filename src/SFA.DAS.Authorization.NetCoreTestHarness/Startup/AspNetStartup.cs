using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.Authorization.NetCoreTestHarness.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Startup
{
    public class AspNetStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDasConfiguration()
                .AddDasAuthentication()
                .AddDasAuthorization()
                .AddDasMvc();
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