using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.EmployerFeatures.Errors
{
    public class EmployerFeatureUserNotWhitelisted : AuthorizationError
    {
        public EmployerFeatureUserNotWhitelisted() : base("Employer feature user not whitelisted")
        {
        }
    }
}