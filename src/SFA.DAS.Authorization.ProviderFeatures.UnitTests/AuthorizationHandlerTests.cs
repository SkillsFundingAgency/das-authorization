using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Features;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.ProviderFeatures.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationHandlerTests : FluentTest<EmployerFeaturesAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return TestAsync(f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenAndedOptionsAreAvailable_ThenShouldThrowNotImplementedException()
        {
            return TestExceptionAsync(f => f.SetAndedOptions(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<NotImplementedException>());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOredOptionIsAvailable_ThenShouldThrowNotImplementedException()
        {
            return TestExceptionAsync(f => f.SetOredOption(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<NotImplementedException>());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndFeatureIsEnabledAndWhitelistIsEnabledAndAuthorizationContextIsMissingUkprn_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(
                f => f.SetOption().SetFeatureToggle(true, false, false).SetAuthorizationContextMissingUkprn(), 
                f => f.GetAuthorizationResult(), 
                (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndFeatureIsEnabledAndWhitelistIsEnabledAndAuthorizationContextIsMissingUserEmail_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(
                f => f.SetOption().SetFeatureToggle(true, false, false).SetAuthorizationContextMissingUserEmail(), 
                f => f.GetAuthorizationResult(), 
                (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [TestCase(1, null)]
        [TestCase(1, "")]
        [TestCase(null, "foo@bar.com")]
        [TestCase(null, null)]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndFeatureIsEnabledAndWhitelistIsEnabledAndAuthorizationContextIsAvailableButContainsInvalidValues_ThenShouldThrowInvalidOperationException(long? ukprn, string userEmail)
        {
            return TestExceptionAsync(
                f => f.SetOption().SetFeatureToggle(true, false, false).SetAuthorizationContextValues(ukprn, userEmail), 
                f => f.GetAuthorizationResult(), 
                (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabled_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndUkprnIsWhitelisted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndUkprnIsWhitelistedAndUserEmailIsWhitelisted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, true, true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsNotEnabled_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<ProviderFeatureNotEnabled>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndUkprnIsNotWhitelisted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<ProviderFeatureUserNotWhitelisted>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndUkprnIsWhitelistedAndUserEmailIsNotWhitelisted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, true, false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<ProviderFeatureUserNotWhitelisted>()));
        }
    }

    public class EmployerFeaturesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<IFeatureTogglesService<ProviderFeatureToggle>> FeatureTogglesService { get; set; }
        public Mock<ILogger<AuthorizationHandler>> Logger { get; set; }
        
        public const long Ukprn = 1;
        public const string UserEmail = "foo@bar.com";
        
        public EmployerFeaturesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            FeatureTogglesService = new Mock<IFeatureTogglesService<ProviderFeatureToggle>>();
            Logger = new Mock<ILogger<AuthorizationHandler>>();
            Handler = new AuthorizationHandler(FeatureTogglesService.Object, Logger.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetNonProviderFeatureOptions()
        {
            Options.AddRange(new [] { "Foo", "Bar" });

            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAndedOptions()
        {
            Options.AddRange(new [] { "ProviderRelationships", "Tickles" });
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetOredOption()
        {
            Options.Add($"ProviderRelationships,ProviderRelationships");
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetOption()
        {
            Options.AddRange(new [] { "ProviderRelationships" });

            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAuthorizationContextMissingUkprn()
        {
            AuthorizationContext.Set(AuthorizationContextKey.UserEmail, UserEmail);
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAuthorizationContextMissingUserEmail()
        {
            AuthorizationContext.Set(AuthorizationContextKey.Ukprn, Ukprn);
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAuthorizationContextValues(long? ukprn = Ukprn, string userEmail = UserEmail)
        {
            AuthorizationContext.AddProviderFeatureValues(ukprn, userEmail);
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetFeatureToggle(bool isEnabled, bool? isUkprnWhitelisted = null, bool? isUserEmailWhitelisted = null)
        {
            var option = Options.Single();
            var whitelist = new List<ProviderFeatureToggleWhitelistItem>();

            if (isUkprnWhitelisted != null)
            {
                var userEmails = new List<string>();

                if (isUserEmailWhitelisted != null)
                {
                    userEmails.Add(isUserEmailWhitelisted == true ? UserEmail : "");
                }
                
                whitelist.Add(new ProviderFeatureToggleWhitelistItem { Ukprn = isUkprnWhitelisted == true ? Ukprn : 0, UserEmails = userEmails });
            }

            FeatureTogglesService.Setup(s => s.GetFeatureToggle(option)).Returns(new ProviderFeatureToggle { Feature = "ProviderRelationships", IsEnabled = isEnabled, Whitelist = whitelist });
            
            return this;
        }
    }
}