using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.EmployerFeatures.Errors
{
    public class EmployerFeatureAgreementNotSigned : AuthorizationError
    {
        public EmployerFeatureAgreementNotSigned() : base("Employer feature agreement not signed")
        {
        }
    }
}