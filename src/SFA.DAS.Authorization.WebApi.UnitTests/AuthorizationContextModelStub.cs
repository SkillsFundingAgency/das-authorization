using System;

namespace SFA.DAS.Authorization.WebApi.UnitTests
{
    public class AuthorizationContextModelStub : IAuthorizationContextModel
    {
        public string Foo { get; set; }
        public Guid? UserRef { get; set; }
    }
}