namespace SFA.DAS.Authorization.NetCoreTestHarness.Authorization
{
    public class TestAuthorizationError : AuthorizationError
    {
        public TestAuthorizationError(string message) : base(message)
        {
        }
    }
}