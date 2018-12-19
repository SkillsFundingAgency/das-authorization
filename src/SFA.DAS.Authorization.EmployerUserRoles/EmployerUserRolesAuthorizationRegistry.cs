using StructureMap;

namespace SFA.DAS.Authorization.EmployerRoles
{
    public class EmployerUserRolesAuthorizationRegistry : Registry
    {
        public EmployerUserRolesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
        }
    }
}