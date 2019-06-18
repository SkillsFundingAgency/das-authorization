using Moq;
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
                var commitmentPermissionsApiClientFactory = new Mock<ICommitmentPermissionsApiClientFactory>();
                var commitmentPermissionsApiClient = new Mock<ICommitmentPermissionsApiClient>();

                commitmentPermissionsApiClientFactory.Setup(f => f.CreateClient()).Returns(commitmentPermissionsApiClient.Object);
                
                For<ICommitmentPermissionsApiClientFactory>().Use(commitmentPermissionsApiClientFactory.Object);
            }
        }
    }
}