using System;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.Authorization.Mvc.UnitTests.ModelBinding
{
    public class AuthorizationContextModelStub : IAuthorizationContextModel
    {
        public Guid? UserRef { get; set; }
    }
}