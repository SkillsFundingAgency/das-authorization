using StructureMap;

namespace SFA.DAS.Authorization.EmployerRoles
{
    public class EmployerRolesAuthorizationRegistry : Registry
    {
        public EmployerRolesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
        }
    }
}