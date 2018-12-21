using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.TestHarness.Handlers;
using StructureMap;

namespace SFA.DAS.Authorization.TestHarness.DependencyResolution
{
    public class TestHarnessAuthorizationRegistry : Registry
    {
        public TestHarnessAuthorizationRegistry()
        {
            For<IAuthorizationContextProvider>().Use<TestHarnessAuthorizationContextProvider>();

            //For<IAuthorizationHandler>().Use<TestHarnessAuthorizationHandler>();
            IncludeRegistry<EmployerFeaturesAuthorizationRegistry>();
            //For<IAuthorizationHandler>().Use<EmployerRoles.AuthorizationHandler>();
            //For<IAuthorizationHandler>().Use<ProviderPermissions.AuthorizationHandler>();
        }
    }
}