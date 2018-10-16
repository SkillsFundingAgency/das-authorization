using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.ProviderPermissions.UnitTests
{
    [TestFixture]
    public class ProviderPermissionsAuthorizationHandlerTests : FluentTest<ProviderPermissionsAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndNonProviderPermissionsOptionsAreAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.SetNonProviderPermissionsOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsNotAvailable_ThenShouldThrowAuthorizationContextKeyNotFoundException()
        {
            return RunAsync(f => f.SetProviderPermissionsOptions().SetNonProviderPermissionsContext(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
    }

    public class ProviderPermissionsAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }

        public ProviderPermissionsAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            Handler = new ProviderPermissionsAuthorizationHandler();
        }

        public Task<AuthorizationResult> GetAuthorizationResultAsync()
        {
            return Handler.GetAuthorizationResultAsync(Options, AuthorizationContext);
        }

        public ProviderPermissionsAuthorizationHandlerTestsFixture SetNonProviderPermissionsOptions()
        {
            Options.AddRange(new [] { "Foo", "Bar" });

            return this;
        }

        public ProviderPermissionsAuthorizationHandlerTestsFixture SetProviderPermissionsOptions()
        {
            Options.AddRange(new [] { ProviderPermissions.CreateCohort });

            return this;
        }

        public ProviderPermissionsAuthorizationHandlerTestsFixture SetNonProviderPermissionsContext()
        {
            AuthorizationContext.Set("Foo", 123);
            AuthorizationContext.Set("Bar", 456);

            return this;
        }
    }
}