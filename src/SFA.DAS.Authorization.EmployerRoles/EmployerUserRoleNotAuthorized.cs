namespace SFA.DAS.Authorization.EmployerRoles
{
    public class EmployerUserRoleNotAuthorized : AuthorizationError
    {
        public EmployerUserRoleNotAuthorized() : base("Employer user role is not authorized")
        {
        }
    }
}