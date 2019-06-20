using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.EmployerUserRoles.Errors
{
    public class EmployerUserRoleNotAuthorized : AuthorizationError
    {
        public EmployerUserRoleNotAuthorized() : base("Employer user role is not authorized")
        {
        }
    }
}