﻿using System;

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
            context.Set("Ukprn", (long?)382938712);
            context.Set("AccountLegalEntityId", (long?)114);
            return context;
        }
    } 
}