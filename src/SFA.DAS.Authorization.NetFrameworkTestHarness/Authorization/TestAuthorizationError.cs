using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.Authorization
{
    public class TestAuthorizationError : AuthorizationError
    {
        public TestAuthorizationError(string message) : base(message)
        {
        }
    }
}