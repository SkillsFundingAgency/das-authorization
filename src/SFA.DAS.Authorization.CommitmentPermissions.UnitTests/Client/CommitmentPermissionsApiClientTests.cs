using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.Http;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests.Client
{
    [TestFixture]
    [Parallelizable]
    public class CommitmentPermissionsApiClientTests : FluentTest<CommitmentPermissionsApiClientTestsFixture>
    {
        [TestCase(false)]
        [TestCase(true)]
        public Task CanAccessCohort_WhenRequestingResponse_ThenShouldReturnResponse(bool isAuthorized)
        {
            return TestAsync(
                f => f.SetupResponseForCohortAccessCheck(isAuthorized),
                f => f.CommitmentPermissionsApiClient.CanAccessCohort(f.CohortAccessRequest),
                (f, r) => r.Should().Be(isAuthorized));
        }
    }

    public class CommitmentPermissionsApiClientTestsFixture
    {
        public Mock<IRestHttpClient> RestHttpClient { get; }
        public CohortAccessRequest CohortAccessRequest { get; }
        public CommitmentPermissionsApiClient CommitmentPermissionsApiClient { get; }

        public CommitmentPermissionsApiClientTestsFixture()
        {
            RestHttpClient = new Mock<IRestHttpClient>();
            CohortAccessRequest = new CohortAccessRequest();
            CommitmentPermissionsApiClient = new CommitmentPermissionsApiClient(RestHttpClient.Object);
        }

        public CommitmentPermissionsApiClientTestsFixture SetupResponseForCohortAccessCheck(bool isAuthorized)
        {
            RestHttpClient.Setup(c => c.Get<bool>("api/authorization/access-cohort", CohortAccessRequest, CancellationToken.None)).ReturnsAsync(isAuthorized);
            
            return this;
        }
    }
}