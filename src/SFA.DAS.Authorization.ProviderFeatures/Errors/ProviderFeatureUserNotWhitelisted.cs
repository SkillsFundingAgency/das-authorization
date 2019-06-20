using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.ProviderFeatures.Errors
{
    public class ProviderFeatureUserNotWhitelisted : AuthorizationError
    {
        public ProviderFeatureUserNotWhitelisted() : base("Provider feature user not whitelisted")
        {
        }
    }
}