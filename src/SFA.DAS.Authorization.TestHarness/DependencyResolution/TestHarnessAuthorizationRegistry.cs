using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFA.DAS.Authorization.TestHarness.Handlers;
using StructureMap;

namespace SFA.DAS.Authorization.TestHarness.DependencyResolution
{
    public class TestHarnessAuthorizationRegistry : Registry
    {
        public TestHarnessAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Use<TestHarnessAuthorizationHandler>();
        }
    }
}