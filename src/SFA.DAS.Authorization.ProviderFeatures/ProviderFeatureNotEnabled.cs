namespace SFA.DAS.Authorization.ProviderFeatures
{
    public class ProviderFeatureNotEnabled : AuthorizationError
    {
        public ProviderFeatureNotEnabled() : base("Provider feature is not enabled")
        {
        }
    }
}