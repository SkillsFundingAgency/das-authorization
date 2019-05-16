using System;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    public class AuthorizationContextModelStub : IAuthorizationContextModel
    {
        public string UserRef { get; set; }
    }
}