namespace SFA.DAS.Authorization.UnitTests
{
    public sealed class EmployerFeatureDisabled : AuthorizationError
    {
        public EmployerFeatureDisabled() : base("Employer feature is disabled")
        {
        }
    }
}