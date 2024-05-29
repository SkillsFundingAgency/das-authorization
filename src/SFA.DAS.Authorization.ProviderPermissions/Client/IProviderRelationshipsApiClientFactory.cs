namespace SFA.DAS.Authorization.ProviderPermissions.Client
{
    public interface IProviderRelationshipsApiClientFactory
    {
        IProviderRelationshipsApiClient CreateClient();
    }
}