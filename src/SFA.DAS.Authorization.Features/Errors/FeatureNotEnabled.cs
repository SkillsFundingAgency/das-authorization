using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.Features.Errors
{
    public class FeatureNotEnabled : AuthorizationError
    {
        public FeatureNotEnabled() : base("Feature is not enabled")
        {
        }
    }
}