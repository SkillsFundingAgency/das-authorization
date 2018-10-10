namespace SFA.DAS.Authorization.UnitTests
{
    public sealed class EmployerRoleNotAuthorized : AuthorizationError
    {
        public EmployerRoleNotAuthorized() : base("Employer role is not authorized")
        {
        }
    }
}