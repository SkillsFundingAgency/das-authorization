﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.EmployerFeatures.Context;
using SFA.DAS.Authorization.EmployerFeatures.Errors;
using SFA.DAS.Authorization.EmployerFeatures.Models;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.Testing;
using AuthorizationHandler = SFA.DAS.Authorization.EmployerFeatures.Handlers.AuthorizationHandler;

namespace SFA.DAS.Authorization.EmployerFeatures.UnitTests.Handlers
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
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndFeatureIsEnabledAndWhitelistIsEnabledAndAuthorizationContextIsIsMissingAccountId_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(
                f => f.SetOption().SetFeatureToggle(true, false, false).SetAuthorizationContextMissingAccountId(), 
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

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabled_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndAccountIdIsWhitelisted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndAccountIdIsWhitelistedAndUserEmailIsWhitelisted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, true, true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsNotEnabled_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<EmployerFeatureNotEnabled>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndAccountIdIsNotWhitelisted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<EmployerFeatureUserNotWhitelisted>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndAccountIdIsWhitelistedAndUserEmailIsNotWhitelisted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true, true, false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<EmployerFeatureUserNotWhitelisted>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndAgreementValuesOnContextAndAgreementHasNotBeenSigned_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true).SetAuthorizationContextAgreementValues().SetAgreementNotBeenSigned(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<EmployerFeatureAgreementNotSigned>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableAndFeatureIsEnabledAndAgreementValuesOnContextAndAgreementHasBeenSigned_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetFeatureToggle(true).SetAuthorizationContextAgreementValues().SetAgreementHasBeenSigned(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }
    }

    public class EmployerFeaturesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<IFeatureTogglesService<EmployerFeatureToggle>> FeatureTogglesService { get; set; }
        public Mock<IEmployerAccountsApiClient> EmployerAccountsApiClient { get; set; }

        public const long AccountId = 1;
        public const string UserEmail = "foo@bar.com";
        public const string AgreementType = "Levy";
        public const int AgreementVersion = 4;
        
        public EmployerFeaturesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            FeatureTogglesService = new Mock<IFeatureTogglesService<EmployerFeatureToggle>>();
            EmployerAccountsApiClient = new Mock<IEmployerAccountsApiClient>();
            Handler = new AuthorizationHandler(FeatureTogglesService.Object, EmployerAccountsApiClient.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
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

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAuthorizationContextValues(long accountId = AccountId, string userEmail = UserEmail)
        {
            AuthorizationContext.AddEmployerFeatureValues(accountId, userEmail);
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAuthorizationContextAgreementValues()
        {
            AuthorizationContext.Set(AuthorizationContextKey.AgreementType, AgreementType);
            AuthorizationContext.Set(AuthorizationContextKey.AgreementVersion, AgreementVersion);

            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetFeatureToggle(bool isEnabled, bool? isAccountIdWhitelisted = null, bool? isUserEmailWhitelisted = null)
        {
            var option = Options.Single();
            var whitelist = new List<EmployerFeatureToggleWhitelistItem>();

            if (isAccountIdWhitelisted != null)
            {
                var userEmails = new List<string>();

                if (isUserEmailWhitelisted != null)
                {
                    userEmails.Add(isUserEmailWhitelisted == true ? UserEmail : "");
                }
                
                whitelist.Add(new EmployerFeatureToggleWhitelistItem { AccountId = isAccountIdWhitelisted == true ? AccountId : 0, UserEmails = userEmails });
            }

            FeatureTogglesService.Setup(s => s.GetFeatureToggle(option)).Returns(new EmployerFeatureToggle { Feature = "ProviderRelationships", IsEnabled = isEnabled, Whitelist = whitelist });
            
            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAgreementNotBeenSigned()
        {
            EmployerAccountsApiClient.Setup(x =>
                    x.HasAgreementBeenSigned(It.IsAny<HasAgreementBeenSignedRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            return this;
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetAgreementHasBeenSigned()
        {
            EmployerAccountsApiClient.Setup(x =>
                    x.HasAgreementBeenSigned(It.IsAny<HasAgreementBeenSignedRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            return this;
        }
    }
}