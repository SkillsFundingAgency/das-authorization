using System;

namespace SFA.DAS.Authorization.TestHarness.Handlers
{
    public class TestHarnessAuthorizationContextProvider : IAuthorizationContextProvider
    {
        public IAuthorizationContext GetAuthorizationContext()
        {
            var context = new AuthorizationContext();
            context.Set("AccountId", (long?)112);
            context.Set("UserEmail", "test");
            context.Set("UserRef", (Guid?)Guid.NewGuid());
            return context;
        }
    }
}