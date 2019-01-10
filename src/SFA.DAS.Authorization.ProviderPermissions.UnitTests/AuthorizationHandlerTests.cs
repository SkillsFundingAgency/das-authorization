using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.ProviderPermissions.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationHandlerTests : FluentTest<AuthorizationHandlerTestsFixture>
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
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingAccountLegalEntityId_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextMissingAccountLegalEntityId(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingUkprn_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextMissingUkprn(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
        
        [TestCase(1L, null)]
        [TestCase(null, 1L)]
        [TestCase(null, null)]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableButContainsInvalidValues_ThenShouldThrowInvalidOperationException(long? accountLegalEntityId, long? ukprn)
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextValues(accountLegalEntityId, ukprn), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndCreateCohortPermissionIsGranted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetPermissionGranted(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndCreateCohortPermissionIsNotGranted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetPermissionGranted(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<ProviderPermissionNotGranted>()));
        }
    }

    public class AuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<IProviderRelationshipsApiClient> ProviderRelationshipsApiClient { get; set; }
        
        public const long AccountLegalEntityId = 22L;
        public const long Ukprn = 333L;
        
        public AuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            ProviderRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient>();
            Handler = new AuthorizationHandler(ProviderRelationshipsApiClient.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public AuthorizationHandlerTestsFixture SetAndedOptions()
        {
            Options.AddRange(new[] { ProviderOperation.CreateCohortOption, "TickleCohort" });
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetOredOption()
        {
            Options.Add($"{ProviderOperation.CreateCohortOption},{ProviderOperation.CreateCohortOption}");
            
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetOption()
        {
            Options.AddRange(new [] { ProviderOperation.CreateCohortOption });
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetAuthorizationContextMissingAccountLegalEntityId()
        {
            AuthorizationContext.Set(AuthorizationContextKey.Ukprn, Ukprn);
            
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetAuthorizationContextMissingUkprn()
        {
            AuthorizationContext.Set(AuthorizationContextKey.AccountLegalEntityId, AccountLegalEntityId);
            
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetAuthorizationContextValues(long? accountLegalEntityId = AccountLegalEntityId, long? ukprn = Ukprn)
        {
            AuthorizationContext.AddProviderPermissionValues(accountLegalEntityId, ukprn);
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetPermissionGranted(bool result)
        {
            var option = Options.Select(o => o.ToEnum<Operation>()).Single();
            
            ProviderRelationshipsApiClient.Setup(c => c.HasPermission(
                    It.Is<HasPermissionRequest>(r => 
                        r.AccountLegalEntityId == AccountLegalEntityId &&
                        r.Ukprn == Ukprn &&
                        r.Operation == option),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            return this;
        }
    }
}