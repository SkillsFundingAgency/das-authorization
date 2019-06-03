namespace SFA.DAS.Authorization.CommitmentPermissions.Client
{

    public interface ICommitmentPermissionsApiClientFactory
    {
        ICommitmentPermissionsApiClient CreateClient();
    }

}
