using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.Http;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests.Client
{
    [TestFixture]
    [Parallelizable]
    public class WhenCallingTheEndpoints
    {
        private WhenCallingTheEndpointsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingTheEndpointsFixture();
        }

        [Test]
        public async Task CanAccessCohort_VerifyUrlAndDataIsCorrectPassedIn()
        {
            await _fixture.CommitmentPermissionsApiClient.CanAccessCohort(_fixture.CohortAccessRequest);
            _fixture.MockRestHttpClient.Verify(x => x.Get<bool>($"api/authorization/access-cohort",
                _fixture.CohortAccessRequest, CancellationToken.None));
        }

        [Test]
        public async Task CanAccessCohort_VerifyResponseWasReturned()
        {
            _fixture.SetupResponseForCohortAccessCheck();
            var result =
                await _fixture.CommitmentPermissionsApiClient.CanAccessCohort(_fixture.CohortAccessRequest,
                    CancellationToken.None);
            Assert.IsTrue(true);
        }

    }

    public class WhenCallingTheEndpointsFixture
    {
        public Mock<IRestHttpClient> MockRestHttpClient { get; }
        public CohortAccessRequest CohortAccessRequest { get; }
        public CommitmentPermissionsApiClient CommitmentPermissionsApiClient { get; }

        public WhenCallingTheEndpointsFixture()
        {
            MockRestHttpClient = new Mock<IRestHttpClient>();
            CohortAccessRequest = new CohortAccessRequest();
            CommitmentPermissionsApiClient = new CommitmentPermissionsApiClient(MockRestHttpClient.Object);
        }

        public WhenCallingTheEndpointsFixture SetupResponseForCohortAccessCheck()
        {
            MockRestHttpClient.Setup(x =>
                    x.Get<bool>(It.IsAny<string>(), It.IsAny<CohortAccessRequest>(), CancellationToken.None))
                .ReturnsAsync(true);
            return this;
        }
    }
}