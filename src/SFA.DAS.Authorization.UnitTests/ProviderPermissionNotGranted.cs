namespace SFA.DAS.Authorization.UnitTests
{
    public sealed class ProviderPermissionNotGranted : AuthorizationError
    {
        public ProviderPermissionNotGranted() : base("Provider permission is not granted")
        {
        }
    }
}