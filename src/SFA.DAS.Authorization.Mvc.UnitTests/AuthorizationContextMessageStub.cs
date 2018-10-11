using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    public class AuthorizationContextMessageStub : IAuthorizationContextMessage
    {
        [Required]
        public string Foo { get; set; }

        [Required]
        public Guid? UserRef { get; set; }
    }
}