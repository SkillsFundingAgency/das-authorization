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
        [Test]
        public Task WhenOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.SetNonProviderPermissionsOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsNotAvailable_ThenShouldThrowAuthorizationContextKeyNotFoundException()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetNonProviderPermissionsContext(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

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
        
        public ProviderPermissionsAuthorizationHandlerTestsFixture SetProviderPermissionsContext()
        {
            AuthorizationContext.Add("AccountLegalEntityId", AccountLegalEntityId);
            AuthorizationContext.Add("ProviderId", ProviderId);
            
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