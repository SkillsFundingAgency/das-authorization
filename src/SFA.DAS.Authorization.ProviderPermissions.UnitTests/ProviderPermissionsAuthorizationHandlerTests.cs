using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.ProviderPermissions.UnitTests
{
    [TestFixture]
    public class ProviderPermissionsAuthorizationHandler_GetAuthorizationResultAsync_Tests : FluentTest<ProviderPermissionsAuthorizationHandlerTestsFixture>
    {
        #region No Provider Permissions Options

        [Test]
        public Task WhenNoOptions_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResultAsync(), 
                (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task WhenOnlyNonProviderPermissionsOptionsAreAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.SetNonProviderPermissionsOptions(), 
                f => f.GetAuthorizationResultAsync(), 
                (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        #endregion No Provider Permissions Options
        
        #region Missing Context
        
        [TestCase(1L, null)]
        [TestCase(null, 1L)]
        [TestCase(null, null)]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsAvailableButContainsNulls_ThenShouldThrowArgumentNullException(
            long? accountLegalEntityId, long? providerId)
        {
            return RunAsync(
                f => f.SetProviderPermissionsCreateCohortOption()
                    .SetProviderPermissionsContext(accountLegalEntityId, providerId),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<ArgumentNullException>());
//                (f, r) => r.Should().Throw<ArgumentNullException>().WithMessage("**"));
//                (f, r) => r.Should().Throw<ArgumentNullException>().And.Message.MatchRegex(""));
//                (f, r) => r.Should().Throw<ArgumentNullException>().And.Message.Should().MatchRegex(""));
//                (f, r) => r.Should().Throw<ArgumentNullException>().Which.Message.Should().MatchRegex(""));
//                (f, r) => r.Should().Throw<ArgumentNullException>().Where(ex => ex.Message.Should().MatchRegex("")));
        }

        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableButOnlyNonProviderPermissionsContextIsAvailable_ThenShouldThrowKeyNotFoundException()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetNonProviderPermissionsContext(), 
                f => f.GetAuthorizationResultAsync(), 
                (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsMissingAccountLegalEntityId_ThenShouldThrowKeyNotFoundException()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetProviderIdProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsMissingProviderId_ThenShouldReturnNotAuthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetAccountLegalEntityIdProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        #endregion Missing Context
        
        #region Permission Checked
        
        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsAvailableAndCreateCohortPermissionIsGranted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetProviderPermissionsContext().MakeCreateCohortPermissionCheckReturn(true),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }
        
        [Test, Ignore("Until client has HasPermissions()")]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsAvailableAndCreateCohortPermissionIsNotGranted_ThenShouldReturnNotAuthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetProviderPermissionsContext().MakeCreateCohortPermissionCheckReturn(false),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized));
        }
        
        [Test, Ignore("Until client has HasPermissions()")]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsAvailableAndCreateCohortPermissionIsNotGranted_ThenShouldReturnAuthorizationResultWithProviderPermissionNotGrantedError()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Errors.Should().NotBeNull().And.BeEquivalentTo(new ProviderPermissionNotGranted()));
        }
        
        #endregion Permission Checked
    }

    public class ProviderPermissionsAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<IProviderRelationshipsApiClient> ProviderRelationshipsApiClient { get; set; }
        
        public const long AccountLegalEntityId = 22L;
        public const long ProviderId = 333L;
        
        public ProviderPermissionsAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            ProviderRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient>();
            Handler = new ProviderPermissionsAuthorizationHandler(ProviderRelationshipsApiClient.Object);
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

        public ProviderPermissionsAuthorizationHandlerTestsFixture SetProviderPermissionsCreateCohortOption()
        {
            Options.AddRange(new [] { ProviderPermissions.CreateCohort });
            return this;
        }

        public ProviderPermissionsAuthorizationHandlerTestsFixture SetNonProviderPermissionsContext()
        {
            AuthorizationContext.Add("Foo", 123);
            AuthorizationContext.Add("Bar", 456);
            return this;
        }
        
        public ProviderPermissionsAuthorizationHandlerTestsFixture SetProviderPermissionsContext(long? accountLegalEntityId = AccountLegalEntityId, long? providerId = ProviderId)
        {
            AuthorizationContext.Add("AccountLegalEntityId", accountLegalEntityId);
            AuthorizationContext.Add("ProviderId", providerId);
            return this;
        }
        
        public ProviderPermissionsAuthorizationHandlerTestsFixture SetAccountLegalEntityIdProviderPermissionsContext(long? accountLegalEntityId = AccountLegalEntityId)
        {
            AuthorizationContext.Add("AccountLegalEntityId", accountLegalEntityId);
            return this;
        }

        public ProviderPermissionsAuthorizationHandlerTestsFixture SetProviderIdProviderPermissionsContext(long? providerId = ProviderId)
        {
            AuthorizationContext.Add("ProviderId", providerId);
            return this;
        }
        
        public ProviderPermissionsAuthorizationHandlerTestsFixture MakeCreateCohortPermissionCheckReturn(bool result)
        {
//            ProviderRelationshipsApiClient.Setup(c => c.HasPermissions(It.Is<HasPermissionsRequest>(
//                    r => r.AccountLegalEntityId == AccountLegalEntityId
//                         && r.ProviderId == ProviderId
//                         && r.Permissions != null && r.Permissions.Count == 1 &&
//                         r.Permissions[0] == "ProviderPermissions.CreateCohort")))
//                .ReturnsAsync(result);
            
            return this;
        }
    }
}