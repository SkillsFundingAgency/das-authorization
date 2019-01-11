namespace SFA.DAS.Authorization.EmployerUserRoles
{
    public class EmployerUserRoleNotAuthorized : AuthorizationError
    {
        public EmployerUserRoleNotAuthorized() : base("Employer user role is not authorized")
        {
        }
    }
}