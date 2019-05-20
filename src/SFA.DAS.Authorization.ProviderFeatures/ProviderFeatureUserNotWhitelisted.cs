namespace SFA.DAS.Authorization.ProviderFeatures
{
    public class ProviderFeatureUserNotWhitelisted : AuthorizationError
    {
        public ProviderFeatureUserNotWhitelisted() : base("Provider feature user not whitelisted")
        {
        }
    }
}