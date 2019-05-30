namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public class CommitmentPermissionNotGranted : AuthorizationError
    {
        public CommitmentPermissionNotGranted() : base("Commitment permission is not granted")
        {
        }
    }
}