using System;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    public class AuthorizationContextModelStub : IAuthorizationContextModel
    {
        public Guid? UserRef { get; set; }
    }
}