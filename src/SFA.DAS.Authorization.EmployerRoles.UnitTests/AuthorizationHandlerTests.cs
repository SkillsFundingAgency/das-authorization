using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.EmployerRoles.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationHandlerTests : FluentTest<EmployerRolesAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return TestAsync(f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test, Ignore("until handler written")]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndNonEmployerRolesOptionsAreAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return TestAsync(f => f.SetNonEmployerRolesOptions(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndEmployerRolesOptionsAreAvailableAndEmployerRoleContextIsNotAvailable_ThenShouldThrowAuthorizationContextKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetEmployerRolesOptions(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
    }

    public class EmployerRolesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<ILogger> Logger { get; set; }

        public EmployerRolesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            Logger = new Mock<ILogger>();
            Handler = new AuthorizationHandler(Logger.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetNonEmployerRolesOptions()
        {
            Options.AddRange(new [] { "Foo", "Bar" });

            return this;
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetEmployerRolesOptions()
        {
            Options.AddRange(new [] { EmployerRole.Owner, EmployerRole.Transactor });

            return this;
        }
    }
}