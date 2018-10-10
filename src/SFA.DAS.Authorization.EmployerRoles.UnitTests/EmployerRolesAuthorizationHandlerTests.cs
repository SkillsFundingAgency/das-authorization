using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.EmployerRoles.UnitTests
{
    [TestFixture]
    public class EmployerRolesAuthorizationHandlerTests : FluentTest<EmployerRolesAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndNonEmployerRoleOptionsAreAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.SetNonEmployerRolesOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndEmployerRoleOptionsAreAvailableAndEmployerRoleContextIsNotAvailable_ThenShouldThrowAuthorizationContextKeyNotFoundException()
        {
            return RunAsync(f => f.SetEmployerRolesOptions().SetNonEmployerRolesContext(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
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
            Handler = new EmployerRolesAuthorizationHandler();
        }

        public Task<AuthorizationResult> GetAuthorizationResultAsync()
        {
            return Handler.GetAuthorizationResultAsync(Options, AuthorizationContext);
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetNonEmployerRolesOptions()
        {
            Options.AddRange(new [] { "Foo", "Bar" });

            return this;
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetEmployerRolesOptions()
        {
            Options.AddRange(new [] { EmployerRoles.Owner, EmployerRoles.Transactor });

            return this;
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetNonEmployerRolesContext()
        {
            AuthorizationContext.Set("Foo", 123);
            AuthorizationContext.Set("Bar", 456);

            return this;
        }
    }
}