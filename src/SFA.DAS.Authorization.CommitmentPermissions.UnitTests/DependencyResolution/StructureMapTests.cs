using NUnit.Framework;
using SFA.DAS.Authorization.CommitmentPermissions.Configuration;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.Authorization.DependencyResolution.StructureMap;
using StructureMap;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests.DependencyResolution
{
    [TestFixture]
    [Parallelizable]
    public class StructureMapTests
    {
        [Test]
        public void AssertConfigurationIsValid()
        {
            using (var container = new Container(c =>
            {
                c.AddRegistry<AuthorizationRegistry>();
                c.AddRegistry<CommitmentPermissionsAuthorizationRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MemoryCacheRegistry>();
                c.AddRegistry<OptionsRegistry>();
                c.AddRegistry<DefaultRegistry>();
            }))
            {
                container.AssertConfigurationIsValid();
            }
        }

        private class DefaultRegistry : Registry
        {
            public DefaultRegistry()
            {
                For<CommitmentPermissionsApiClientConfiguration>().Use(new CommitmentPermissionsApiClientConfiguration
                {
                    ApiBaseUrl = "https://localhost",
                    IdentifierUri = ""
                });
            }
        }
    }
}