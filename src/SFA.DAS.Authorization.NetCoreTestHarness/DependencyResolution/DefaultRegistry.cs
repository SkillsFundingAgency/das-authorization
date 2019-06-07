using Microsoft.Extensions.Configuration;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Http.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using StructureMap;

namespace SFA.DAS.Authorization.NetCoreTestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            var authSection = "SFA.DAS.ProviderCommitments:CommitmentsClientApi";
            For<IAzureActiveDirectoryClientConfiguration>()
                .Use(c => GetInstance<AzureActiveDirectoryClientConfiguration>(c, authSection));
            For<ICommitmentPermissionsApiClientFactory>().Use<CommitmentPermissionsApiClientFactory>();
        }

        private T GetInstance<T>(IContext context, string name)
        {
            var configuration = context.GetInstance<IConfiguration>();
            var configSection = configuration.GetSection(name);
            var t = configSection.Get<T>();
            return t;
        }
    }
}