namespace SFA.DAS.Authorization.UnitTests
{
    public sealed class EmployerFeatureDisabledAuthorizationError : AuthorizationError
    {
        public EmployerFeatureDisabledAuthorizationError() : base("Employer feature is disabled")
        {
        }
    }
}