using StructureMap;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesAuthorizationRegistry : Registry
    {
        public EmployerFeaturesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<EmployerFeaturesAuthorizationHandler>();
        }
    }
}