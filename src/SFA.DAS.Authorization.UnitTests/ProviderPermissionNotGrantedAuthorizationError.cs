namespace SFA.DAS.Authorization.UnitTests
{
    public sealed class ProviderPermissionNotGrantedAuthorizationError : AuthorizationError
    {
        public ProviderPermissionNotGrantedAuthorizationError() : base("Provider permission is not granted")
        {
        }
    }
}