using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.EmployerRoles.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationHandlerTests : FluentTest<EmployerRolesAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test, Ignore("until handler written")]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndNonEmployerRolesOptionsAreAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.SetNonEmployerRolesOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndEmployerRolesOptionsAreAvailableAndEmployerRoleContextIsNotAvailable_ThenShouldThrowAuthorizationContextKeyNotFoundException()
        {
            return RunAsync(f => f.SetEmployerRolesOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
    }

    public class EmployerRolesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }

        public EmployerRolesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            Handler = new AuthorizationHandler();
        }

        public Task<AuthorizationResult> GetAuthorizationResultAsync()
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