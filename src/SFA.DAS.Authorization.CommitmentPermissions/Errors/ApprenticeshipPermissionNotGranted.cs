using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.CommitmentPermissions.Errors
{
    public class ApprenticeshipPermissionNotGranted : AuthorizationError
    {
        public ApprenticeshipPermissionNotGranted() : base("Apprenticeship permission is not granted")
        {
        }
    }
}