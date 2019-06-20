using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Authorization;
using StructureMap;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IAuthorizationContextProvider>().Use<TestAuthorizationContextProvider>();
            For<IAuthorizationHandler>().Add<TestAuthorizationHandler>();;
            
            Scan(s =>
            {
                s.TheCallingAssembly();
                s.WithDefaultConventions();
                s.With(new ControllerConvention());
            });
        }
    }
}