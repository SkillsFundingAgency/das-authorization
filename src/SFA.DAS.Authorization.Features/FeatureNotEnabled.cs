namespace SFA.DAS.Authorization.Features
{
    public class FeatureNotEnabled : AuthorizationError
    {
        public FeatureNotEnabled() : base("Feature is not enabled")
        {
        }
    }
}