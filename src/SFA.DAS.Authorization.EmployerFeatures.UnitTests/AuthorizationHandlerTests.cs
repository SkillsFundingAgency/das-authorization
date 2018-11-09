using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.EmployerFeatures.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationHandlerTests : FluentTest<EmployerFeaturesAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }
        
        [Test]
        public Task GetAuthorizationResultAsync_WhenAndedOptionsAreAvailable_ThenShouldThrowNotImplementedException()
        {
            return RunAsync(f => f.SetAndedOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<NotImplementedException>());
        }
        
        [Test]
        public Task GetAuthorizationResultAsync_WhenOredOptionIsAvailable_ThenShouldThrowNotImplementedException()
        {
            return RunAsync(f => f.SetOredOption(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<NotImplementedException>());
        }
        
        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsIsMissingAccountId_ThenShouldThrowKeyNotFoundException()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextMissingAccountId(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsMissingUserEmail_ThenShouldThrowKeyNotFoundException()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextMissingUserEmail(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [TestCase(1, null)]
        [TestCase(1, "")]
        [TestCase(null, "foo@bar.com")]
        [TestCase(null, null)]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableButContainsInvalidValues_ThenShouldThrowInvalidOperationException(long? accountId, string userEmail)
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextValues(accountId, userEmail), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndUserEmailIsWhitelisted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, true), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsNotEnabled_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(false), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<EmployerFeatureNotEnabled>()));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndUserEmailIsNotWhitelisted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<EmployerFeatureUserNotWhitelisted>()));
        }
    }

    public class EmployerFeaturesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<IFeatureTogglesService> FeatureTogglesService { get; set; }
        
        public const long AccountId = 1;
        public const string UserEmail = "foo@bar.com";
        
        public EmployerFeaturesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            FeatureTogglesService = new Mock<IFeatureTogglesService>();
            Handler = new AuthorizationHandler(FeatureTogglesService.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResultAsync()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetNonEmployerFeatureOptions()
        {
            Options.AddRange(new [] { "Foo", "Bar" });

            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAndedOptions()
        {
            Options.AddRange(new [] { EmployerFeature.ProviderRelationshipsOption, "Tickles" });
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetOredOption()
        {
            Options.Add($"{EmployerFeature.ProviderRelationshipsOption},{EmployerFeature.ProviderRelationshipsOption}");
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetOption()
        {
            Options.AddRange(new [] { EmployerFeature.ProviderRelationshipsOption });

            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAuthorizationContextMissingAccountId()
        {
            AuthorizationContext.Set(AuthorizationContextKey.UserEmail, UserEmail);
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAuthorizationContextMissingUserEmail()
        {
            AuthorizationContext.Set(AuthorizationContextKey.AccountId, AccountId);
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAuthorizationContextValues(long? accountId = AccountId, string userEmail = UserEmail)
        {
            AuthorizationContext.AddEmployerFeatureValues(accountId, userEmail);
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetFeatureToggle(bool isEnabled, bool isUserEmailWhitelisted = false)
        {
            var option = Options.Select(o => o.ToEnum<Feature>()).Single();
            var userEmailWhitelist = new List<string>();

            userEmailWhitelist.Add(isUserEmailWhitelisted ? UserEmail : "_");

            FeatureTogglesService.Setup(s => s.GetFeatureToggle(option))
                .Returns(new FeatureToggle(Feature.ProviderRelationships, isEnabled, userEmailWhitelist));
            
            return this;
        }
    }
}