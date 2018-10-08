namespace SFA.DAS.Authorization.EmployerRoles
{
    public class EmployerRoleNotAuthorized : AuthorizationError
    {
        public EmployerRoleNotAuthorized() : base("Employer role was not authorized")
        {
        }
    }
}