using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.EmployerFeatures.Errors
{
    public class EmployerFeatureNotEnabled : AuthorizationError
    {
        public EmployerFeatureNotEnabled() : base("Employer feature is not enabled")
        {
        }
    }
}