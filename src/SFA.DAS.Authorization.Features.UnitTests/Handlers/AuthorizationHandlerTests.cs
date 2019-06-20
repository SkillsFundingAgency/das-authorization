using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Features.Errors;
using SFA.DAS.Authorization.Features.Handlers;
using SFA.DAS.Authorization.Features.Models;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Features.UnitTests.Handlers
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
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndFeatureIsEnabled_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetFeatureToggle(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndFeatureIsNotEnabled_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetFeatureToggle(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<FeatureNotEnabled>()));
        }
    }

    public class EmployerFeaturesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<IFeatureTogglesService<FeatureToggle>> FeatureTogglesService { get; set; }
        public Mock<ILogger<AuthorizationHandler>> Logger { get; set; }
        
        public EmployerFeaturesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            FeatureTogglesService = new Mock<IFeatureTogglesService<FeatureToggle>>();
            Logger = new Mock<ILogger<AuthorizationHandler>>();
            Handler = new AuthorizationHandler(FeatureTogglesService.Object, Logger.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetNonFeatureOptions()
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

        public EmployerFeaturesAuthorizationHandlerTestsFixture SetFeatureToggle(bool isEnabled, bool? isAccountIdWhitelisted = null, bool? isUserEmailWhitelisted = null)
        {
            var option = Options.Single();

            FeatureTogglesService.Setup(s => s.GetFeatureToggle(option)).Returns(new FeatureToggle { Feature = "ProviderRelationships", IsEnabled = isEnabled });
            
            return this;
        }
    }
}