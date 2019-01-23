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
            context.Set("UserRef", (Guid?) new Guid("121AF597-42D1-4AEE-96BE-D1F88293B140"));
            context.Set("Ukprn", (long?)382938712);
            context.Set("AccountLegalEntityId", (long?)114);
            return context;
        }
    } 
}