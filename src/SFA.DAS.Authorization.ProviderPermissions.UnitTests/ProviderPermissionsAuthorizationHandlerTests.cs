using System;
using System.Collections.Generic;
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
    public class AuthorizationHandler_GetAuthorizationResultAsync_Tests : FluentTest<AuthorizationHandlerTestsFixture>
    {
        #region No Options

        [Test]
        public Task WhenNoOptions_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResultAsync(), 
                (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }
        
        #endregion No Options
        
        #region Unsupported Options

        [Test]
        public Task WhenMoreThanOneProviderOperationOptionIsSuppliedAndContextIsAvailable_ThenShouldThrowANotImplementedException()
        {
            return RunAsync(f => f.SetTwoOptions().SetProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<NotImplementedException>());
        }

        [Test]
        public Task WhenCommaSeparatedOptionIsSuppliedAndContextIsAvailable_ThenShouldThrowANotImplementedException()
        {
            return RunAsync(f => f.SetOredOption().SetProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<NotImplementedException>());
        }
        
        [Test]
        public Task WhenANonProviderPermissionsOptionIsIncorrectlySuppliedAndContextIsAvailable_ThenShouldThrowAnException()
        {
            return RunAsync(f => f.SetANonProviderOpertionOption().SetProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<Exception>()); //.WithMessage("Requested value 'Foo' was not found."));
        }

        [Test]
        public Task WhenANonProviderPermissionsOptionIsIncorrectlySuppliedAndContextIsNotAvailable_ThenShouldThrowAnException()
        {
            return RunAsync(f => f.SetANonProviderOpertionOption(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<Exception>());
        }
        
        #endregion Unsupported Options
        
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
                (f, r) => r.Should().Throw<InvalidOperationException>());
//                (f, r) => r.Should().Throw<ArgumentNullException>().WithMessage("**"));
//                (f, r) => r.Should().Throw<ArgumentNullException>().And.Message.MatchRegex(""));
//                (f, r) => r.Should().Throw<ArgumentNullException>().And.Message.Should().MatchRegex(""));
//                (f, r) => r.Should().Throw<ArgumentNullException>().Which.Message.Should().MatchRegex(""));
//                (f, r) => r.Should().Throw<ArgumentNullException>().Where(ex => ex.Message.Should().MatchRegex("")));
        }

        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableButOnlyNonProviderPermissionsContextIsAvailable_ThenShouldThrowKeyNotFoundException()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption(), 
                f => f.GetAuthorizationResultAsync(), 
                (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsMissingAccountLegalEntityId_ThenShouldThrowKeyNotFoundException()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetProviderIdProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsMissingProviderId_ThenShouldReturnNotAuthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetAccountLegalEntityIdProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().Throw<InvalidOperationException>());
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
        
        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsAvailableAndCreateCohortPermissionIsNotGranted_ThenShouldReturnNotAuthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetProviderPermissionsContext().MakeCreateCohortPermissionCheckReturn(false),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized));
        }
        
        [Test]
        public Task WhenProviderPermissionsOptionsAreAvailableAndProviderPermissionsContextIsAvailableAndCreateCohortPermissionIsNotGranted_ThenShouldReturnAuthorizationResultWithProviderPermissionNotGrantedError()
        {
            return RunAsync(f => f.SetProviderPermissionsCreateCohortOption().SetProviderPermissionsContext(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Errors.Should().NotBeNull().And.BeEquivalentTo(new ProviderPermissionNotGranted()));
        }
        
        #endregion Permission Checked
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
            return Handler.GetAuthorizationResultAsync(Options, AuthorizationContext);
        }
        
        public AuthorizationHandlerTestsFixture SetANonProviderOpertionOption()
        {
            Options.Add("Foo");
            return this;
        }

        public AuthorizationHandlerTestsFixture SetTwoOptions()
        {
            Options.AddRange(new[] {ProviderOperation.CreateCohortOption, "TickleCohort"});
            return this;
        }

        public AuthorizationHandlerTestsFixture SetOredOption()
        {
            Options.Add($"{ProviderOperation.CreateCohortOption},{ProviderOperation.CreateCohortOption}");
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetProviderPermissionsCreateCohortOption()
        {
            Options.AddRange(new [] { ProviderOperation.CreateCohortOption });
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetProviderPermissionsContext(long? accountLegalEntityId = AccountLegalEntityId, long? ukprn = Ukprn)
        {
            AuthorizationContext.AddProviderPermissionValues(accountLegalEntityId, ukprn);
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetAccountLegalEntityIdProviderPermissionsContext(long? accountLegalEntityId = AccountLegalEntityId)
        {
            AuthorizationContext.AddProviderPermissionValues(accountLegalEntityId, null);
            return this;
        }

        public AuthorizationHandlerTestsFixture SetProviderIdProviderPermissionsContext(long? ukprn = Ukprn)
        {
            AuthorizationContext.AddProviderPermissionValues(null, ukprn);
            return this;
        }
        
        public AuthorizationHandlerTestsFixture MakeCreateCohortPermissionCheckReturn(bool result)
        {
            ProviderRelationshipsApiClient.Setup(c => c.HasPermission(It.Is<PermissionRequest>(
                    r => r.EmployerAccountLegalEntityId == AccountLegalEntityId
                         && r.Ukprn == Ukprn
                         && r.Operation == Operation.CreateCohort), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            return this;
        }
    }
}