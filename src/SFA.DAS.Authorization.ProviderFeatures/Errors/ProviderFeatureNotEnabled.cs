using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.ProviderFeatures.Errors
{
    public class ProviderFeatureNotEnabled : AuthorizationError
    {
        public ProviderFeatureNotEnabled() : base("Provider feature is not enabled")
        {
        }
    }
}