using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Authorization.WebApi.UnitTests
{
    public class AuthorizationContextMessageStub : IAuthorizationContextMessage
    {
        [Required]
        public string Foo { get; set; }

        [Required]
        public Guid? UserRef { get; set; }
    }
}