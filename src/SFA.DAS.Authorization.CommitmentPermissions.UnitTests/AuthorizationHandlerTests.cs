using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests
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
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingCohortId_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextMissingCohortId(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingPartyType_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextMissingPartyType(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingPartyId_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextMissingPartyId(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
        
        [TestCase(1L, null, null)]
        [TestCase(null, PartyType.Employer, null)]
        [TestCase(null, null, 1L)]
        [TestCase(null, null, null)]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableButContainsInvalidValues_ThenShouldThrowInvalidOperationException(long? cohortId, PartyType partyType, long? partyId)
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextValues(cohortId, partyType, partyId), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndAccessCohortPermissionIsGranted_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetPermissionGranted(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndAccessCohortPermissionIsNotGranted_ThenShouldReturnUnauthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetPermissionGranted(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => !r2.IsAuthorized && r2.Errors.Count() == 1 && r2.HasError<CommitmentPermissionNotGranted>()));
        }
    }

    public class AuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler Handler { get; set; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
        public Mock<ILogger<AuthorizationHandler>> Logger { get; set; }
        
        public const long CohortId = 1L;
        public const PartyType PartyType = SFA.DAS.Authorization.CommitmentPermissions.PartyType.Employer;
        public const long PartyId = 2L;

        public AuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            Logger = new Mock<ILogger<AuthorizationHandler>>();
            Handler = new AuthorizationHandler(CommitmentsApiClient.Object, Logger.Object);
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
        
        public AuthorizationHandlerTestsFixture SetOption()
        {
            Options.AddRange(new [] { CommitmentOperation.AccessCohortOption });
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetAuthorizationContextMissingCohortId()
        {
            AuthorizationContext.Set(AuthorizationContextKey.PartyType, PartyType);
            AuthorizationContext.Set(AuthorizationContextKey.PartyId, PartyId);
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetAuthorizationContextMissingPartyType()
        {
            AuthorizationContext.Set(AuthorizationContextKey.CohortId, CohortId);
            AuthorizationContext.Set(AuthorizationContextKey.PartyId, PartyId);
            
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetAuthorizationContextMissingPartyId()
        {
            AuthorizationContext.Set(AuthorizationContextKey.CohortId, CohortId);
            AuthorizationContext.Set(AuthorizationContextKey.PartyType, PartyType);
            
            return this;
        }
        
        public AuthorizationHandlerTestsFixture SetAuthorizationContextValues(long? cohortId = CohortId, PartyType partyType = PartyType, long? partyId = PartyId)
        {
            AuthorizationContext.AddCommitmentPermissionValues(cohortId, partyType, partyId);
            
            return this;
        }

        public AuthorizationHandlerTestsFixture SetPermissionGranted(bool result)
        {            
            CommitmentsApiClient.Setup(c => c.CanAccessCohort(
                    It.Is<CanAccessCohortRequest>(r =>
                        r.CohortId == CohortId &&
                        r.PartyType == PartyType &&
                        r.PartyId == PartyId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            return this;
        }
    }
}