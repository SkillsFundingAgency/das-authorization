using System.Web.Http;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.EmployerFeatures.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.EmployerUserRoles.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.Features.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.NetFrameworkTestHarness.DependencyResolution;
using SFA.DAS.Authorization.ProviderFeatures.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.ProviderPermissions.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.WebApi.Extensions;
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