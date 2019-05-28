namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public class CommitmentPermissionNotGranted : AuthorizationError
    {
        public CommitmentPermissionNotGranted(string message) : base("Commitment permission is not granted")
        {
        }
    }
}