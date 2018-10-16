namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeatureUserNotWhitelisted : AuthorizationError
    {
        public EmployerFeatureUserNotWhitelisted() : base("Employer feature user not whitelisted")
        {
        }
    }
}