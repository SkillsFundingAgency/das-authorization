using SFA.DAS.Authorization;

namespace WebApplication3.Handlers
{
    public class TestAuthorizationError : AuthorizationError
    {
        public TestAuthorizationError(string message) : base(message)
        {
        }
    }
}