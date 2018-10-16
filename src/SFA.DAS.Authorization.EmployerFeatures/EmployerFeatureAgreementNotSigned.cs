namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeatureAgreementNotSigned : AuthorizationError
    {
        public EmployerFeatureAgreementNotSigned() : base("Employer feature agreement not signed")
        {
        }
    }
}