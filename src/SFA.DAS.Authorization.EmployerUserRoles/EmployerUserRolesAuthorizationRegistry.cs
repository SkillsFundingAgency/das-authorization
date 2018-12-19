using StructureMap;

namespace SFA.DAS.Authorization.EmployerUserRoles
{
    public class EmployerUserRolesAuthorizationRegistry : Registry
    {
        public EmployerUserRolesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
        }
    }
}