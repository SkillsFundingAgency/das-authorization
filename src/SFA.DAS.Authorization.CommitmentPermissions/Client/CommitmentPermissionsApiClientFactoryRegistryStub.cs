using System;

namespace SFA.DAS.Authorization.CommitmentPermissions.Client
{
    public class CommitmentPermissionsApiClientFactoryRegistryStub : ICommitmentPermissionsApiClientFactory
    {
        public ICommitmentPermissionsApiClient CreateClient()
        {
            throw new NotImplementedException("You must implement ICommitmentPermissionsApiClientFactory or instantiate CommitmentPermissionsApiClientFactory in your consuming " +
                                              "application with the appropriate IAzureActiveDirectoryClientConfiguration config");
        }
    }
}