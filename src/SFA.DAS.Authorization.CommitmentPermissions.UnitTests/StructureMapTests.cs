using NUnit.Framework;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using StructureMap;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests
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
                    Tenant = "",
                    ClientId = "",
                    ClientSecret = "",
                    IdentifierUri = ""
                });
            }
        }
    }
}