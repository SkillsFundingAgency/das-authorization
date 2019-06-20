using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.ProviderPermissions.Errors
{
    public class ProviderPermissionNotGranted : AuthorizationError
    {
        public ProviderPermissionNotGranted() : base("Provider permission is not granted")
        {
        }
    }
}