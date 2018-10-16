namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeatureNotEnabled : AuthorizationError
    {
        public EmployerFeatureNotEnabled() : base("Employer feature is not enabled")
        {
        }
    }
}