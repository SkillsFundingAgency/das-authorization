using System;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    public class AuthorizationContextMessageStub : IAuthorizationContextMessage
    {
        public Guid? UserRef { get; set; }
    }
}