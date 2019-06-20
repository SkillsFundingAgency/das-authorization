using System;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.Authorization.WebApi.UnitTests.ModelBinding
{
    public class AuthorizationContextModelStub : IAuthorizationContextModel
    {
        public string Foo { get; set; }
        public Guid? UserRef { get; set; }
    }
}