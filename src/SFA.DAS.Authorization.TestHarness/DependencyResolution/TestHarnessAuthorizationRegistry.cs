using SFA.DAS.Authorization.TestHarness.Handlers;
using StructureMap;

namespace SFA.DAS.Authorization.TestHarness.DependencyResolution
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