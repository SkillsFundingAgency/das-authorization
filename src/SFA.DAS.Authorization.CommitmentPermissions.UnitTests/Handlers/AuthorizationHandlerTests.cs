using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.CommitmentPermissions.Context;
using SFA.DAS.Authorization.CommitmentPermissions.Errors;
using SFA.DAS.Authorization.CommitmentPermissions.Handlers;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests.Handlers
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
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingCohortIdAndApprenticeshipId_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetAccessCohortOption().SetAuthorizationContextMissingCohortIdAndApprenticeshipId(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task Then_The_AuthorizationResult_Is_Returned_When_No_CohortId_And_Set_To_Ignore_Missing_Params()
        {
            return TestAsync(f => f.SetAccessCohortAndAllowEmptyCohortId().SetAuthorizationContextMissingCohortIdAndApprenticeshipId().SetPermissionGrantedFromNoCohort(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingParty_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetAccessCohortOption().SetAuthorizationContextMissingParty(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingPartyId_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetAccessCohortOption().SetAuthorizationContextMissingPartyId(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndAccessCohortPermissionIsGranted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetAccessCohortOption().SetCohortAuthorizationContextValues().SetCohortPermissionGranted(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndAccessCohortPermissionIsNotGrantedAndAllowedEmptyCohort_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetAccessCohortAndAllowEmptyCohortId().SetCohortAuthorizationContextValues().SetCohortPermissionGranted(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<CommitmentPermissionNotGranted>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndAccessCohortPermissionIsNotGranted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetAccessCohortOption().SetCohortAuthorizationContextValues().SetCohortPermissionGranted(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<CommitmentPermissionNotGranted>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndAccessApprenticeshipPermissionIsGranted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetAccessApprenticeshipOption().SetApprenticeshipAuthorizationContextValues().SetApprenticeshipPermissionGranted(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndAccessApprenticeshipPermissionIsNotGranted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetAccessApprenticeshipOption().SetApprenticeshipAuthorizationContextValues().SetApprenticeshipPermissionGranted(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<ApprenticeshipPermissionNotGranted>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndBothAccessApprenticeshipPermissionAndAccessCohortPermissionAreGranted_ThenShouldReturnAuthorizedAuthorizationResultForApprenticeshipPermission()
        {
            return TestAsync(f => f.SetAccessCohortOption().SetAccessApprenticeshipOption()
                .SetCohortAuthorizationContextValues().SetApprenticeshipAuthorizationContextValues()
                .SetCohortPermissionGranted(true).SetApprenticeshipPermissionGranted(true), 
                f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }
    }

    public class AuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<ICommitmentPermissionsApiClient> CommitmentPermissionsApiClient { get; set; }
        
        public const long CohortId = 1L;
        public const long ApprenticeshipId = 1010L;
        public const Party Party = CommitmentsV2.Types.Party.Employer;
        public const long PartyId = 2;

        public AuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            CommitmentPermissionsApiClient = new Mock<ICommitmentPermissionsApiClient>();
            Handler = new AuthorizationHandler(CommitmentPermissionsApiClient.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public AuthorizationHandlerTestsFixture SetAndedOptions()
        {
            Options.AddRange(new[] { CommitmentOperation.AccessCohortOption, "TickleCohort" });
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetOredOption()
        {
            Options.Add($"{CommitmentOperation.AccessCohortOption},{CommitmentOperation.AccessCohortOption}");
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetAccessCohortAndAllowEmptyCohortId()
        {
            Options.AddRange(new[] { CommitmentOperation.AccessCohortOption, CommitmentOperation.IgnoreEmptyCohortOption });

            return this;
        }

        public AuthorizationHandlerTestsFixture SetAccessCohortOption()
        {
            Options.AddRange(new [] { CommitmentOperation.AccessCohortOption });
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetAccessApprenticeshipOption()
        {
            Options.AddRange(new[] { CommitmentOperation.AccessApprenticeshipOption });

            return this;
        }

        public AuthorizationHandlerTestsFixture SetAuthorizationContextMissingCohortIdAndApprenticeshipId()
        {
            AuthorizationContext.Set(AuthorizationContextKey.Party, Party);
            AuthorizationContext.Set(AuthorizationContextKey.PartyId, PartyId);
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetAuthorizationContextMissingParty()
        {
            AuthorizationContext.Set(AuthorizationContextKey.CohortId, CohortId);
            AuthorizationContext.Set(AuthorizationContextKey.PartyId, PartyId);
            
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetAuthorizationContextMissingPartyId()
        {
            AuthorizationContext.Set(AuthorizationContextKey.CohortId, CohortId);
            AuthorizationContext.Set(AuthorizationContextKey.Party, Party);
            
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetCohortAuthorizationContextValues(long cohortId = CohortId, Party party = Party, long partyId = PartyId)
        {
            AuthorizationContext.AddCommitmentPermissionValues(cohortId, party, partyId);
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetApprenticeshipAuthorizationContextValues(long apprenticeshipId = ApprenticeshipId, Party party = Party, long partyId = PartyId)
        {
            AuthorizationContext.AddApprenticeshipPermissionValues(apprenticeshipId, party, partyId);

            return this;
        }


        public AuthorizationHandlerTestsFixture SetPermissionGrantedFromNoCohort(bool result)
        {
            return this;
        }

        public AuthorizationHandlerTestsFixture SetCohortPermissionGranted(bool result)
        {            
            CommitmentPermissionsApiClient.Setup(c => c.CanAccessCohort(
                    It.Is<CohortAccessRequest>(r =>
                        r.CohortId == CohortId &&
                        r.Party == Party &&
                        r.PartyId == PartyId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetApprenticeshipPermissionGranted(bool result)
        {
            CommitmentPermissionsApiClient.Setup(c => c.CanAccessApprenticeship(
                    It.Is<ApprenticeshipAccessRequest>(r =>
                        r.ApprenticeshipId == ApprenticeshipId &&
                        r.Party == Party &&
                        r.PartyId == PartyId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            return this;
        }
    }
}