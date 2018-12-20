using SFA.DAS.Authorization;
using StructureMap;
using WebApplication3.Handlers;

namespace WebApplication3.DependencyResolution
{
    public class TestHarnessAuthorizationRegistry : Registry
    {
        public TestHarnessAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Use<TestHarnessAuthorizationHandler>();
            //For<Func<IAuthorizationService>>().Use(c => c.GetInstance<IAuthorizationService>());
            For<IAuthorizationContextProvider>().Use<TestHarnessAuthorizationContextProvider>();
        }
    }
}