using System.Web.Http;
using SFA.DAS.Authorization.CommitmentPermissions;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Features;
using SFA.DAS.Authorization.NetFrameworkTestHarness.DependencyResolution;
using SFA.DAS.Authorization.ProviderFeatures;
using SFA.DAS.Authorization.ProviderPermissions;
using SFA.DAS.Authorization.WebApi;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using WebApi.StructureMap;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional }
            );
            
            config.Filters.AddAuthorizationFilter();
            config.Filters.AddUnauthorizedAccessExceptionFilter();
            config.Services.UseAuthorizationModelBinder();
            
            config.UseStructureMap(c =>
            {
                c.AddRegistry<AuthorizationRegistry>();
                c.AddRegistry<AutoConfigurationRegistry>();
                c.AddRegistry<CommitmentPermissionsAuthorizationRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<EmployerFeaturesAuthorizationRegistry>();
                c.AddRegistry<EmployerUserRolesAuthorizationRegistry>();
                c.AddRegistry<FeaturesAuthorizationRegistry>();
                c.AddRegistry<ProviderFeaturesAuthorizationRegistry>();
                c.AddRegistry<ProviderPermissionsAuthorizationRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}