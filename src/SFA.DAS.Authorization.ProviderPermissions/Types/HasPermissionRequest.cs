namespace SFA.DAS.Authorization.ProviderPermissions.Types
{
    public class HasPermissionRequest
    {
        public long Ukprn { get; set; }
        public long AccountLegalEntityId { get; set; }
        public Operation Operations { get; set; }
    }
}