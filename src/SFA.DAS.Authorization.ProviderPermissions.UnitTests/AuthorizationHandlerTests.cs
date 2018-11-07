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
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsMissingAccountLegalEntityId_ThenShouldThrowKeyNotFoundException()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextMissingAccountLegalEntityId(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsMissingUkprn_ThenShouldThrowKeyNotFoundException()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextMissingUkprn(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
        
        [TestCase(1L, null)]
        [TestCase(null, 1L)]
        [TestCase(null, null)]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableButContainsInvalidValues_ThenShouldThrowInvalidOperationException(long? accountLegalEntityId, long? ukprn)
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextValues(accountLegalEntityId, ukprn), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
        
        [Test]
        public Task GetAuthorizationResultAsync_WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsAvailableAndCreateCohortPermissionIsGranted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextValues().SetPermissionGranted(true), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }
        
        [Test]
        public Task GetAuthorizationResultAsync_WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsAvailableAndCreateCohortPermissionIsNotGranted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetOption().SetAuthorizationContextValues().SetPermissionGranted(false), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull()
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

        public Task<AuthorizationResult> GetAuthorizationResultAsync()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }
        
        public AuthorizationHandlerTestsFixture SetNonProviderOpertionOptions()
        {
            Options.AddRange(new [] { "Foo", "Bar" });
            
            return this;
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
                    It.Is<PermissionRequest>(r => 
                        r.EmployerAccountLegalEntityId == AccountLegalEntityId &&
                        r.Ukprn == Ukprn &&
                        r.Operation == option),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            return this;
        }
    }
}