using System;

namespace SFA.DAS.Authorization.WebApi.UnitTests
{
    public class AuthorizationContextMessageStub : IAuthorizationContextMessage
    {
        public string Foo { get; set; }
        public Guid? UserRef { get; set; }
    }
}