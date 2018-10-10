namespace SFA.DAS.Authorization.ProviderPermissions
{
    public class ProviderPermissionNotGranted : AuthorizationError
    {
        public ProviderPermissionNotGranted() : base("Provider permission is not granted")
        {
        }
    }
}