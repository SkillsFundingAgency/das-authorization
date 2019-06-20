using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.CommitmentPermissions.Errors
{
    public class CommitmentPermissionNotGranted : AuthorizationError
    {
        public CommitmentPermissionNotGranted() : base("Commitment permission is not granted")
        {
        }
    }
}