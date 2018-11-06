using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.EmployerFeatures.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class EmployerFeaturesAuthorizationHandlerTests : FluentTest<EmployerFeaturesAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndNonEmployerFeaturesOptionsAreAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.SetNonEmployerFeaturesOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenGettingAuthorizationResultAndEmployerFeaturesOptionsAreAvailableAndEmployerFeaturesContextIsNotAvailable_ThenShouldThrowAuthorizationContextKeyNotFoundException()
        {
            return RunAsync(f => f.SetEmployerFeaturesOptions().SetNonEmployerRolesContext(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
    }

    public class EmployerFeaturesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }

        public EmployerFeaturesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            Handler = new EmployerFeaturesAuthorizationHandler();
        }

        public Task<AuthorizationResult> GetAuthorizationResultAsync()
        {
            return Handler.GetAuthorizationResultAsync(Options, AuthorizationContext);
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetNonEmployerFeaturesOptions()
        {
            Options.AddRange(new [] { "Foo", "Bar" });

            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetEmployerFeaturesOptions()
        {
            Options.AddRange(new [] { EmployerFeatures.ProviderRelationships });

            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetNonEmployerRolesContext()
        {
            AuthorizationContext.Add("Foo", 123);
            AuthorizationContext.Add("Bar", 456);

            return this;
        }
    }
}