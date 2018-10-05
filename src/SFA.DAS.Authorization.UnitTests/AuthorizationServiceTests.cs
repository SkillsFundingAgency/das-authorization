using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.UnitTests
{
    [TestFixture]
    public class AuthorizationServiceTests : FluentTest<AuthorizationServiceTestsFixture>
    {
        [Test]
        public Task IsAuthorizedAsync_WhenOptionsAreAuthorized_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.SetAuthorizedOptions(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public Task IsAuthorizedAsync_WhenOptionsAreUnauthorized_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.SetUnauthorizedOptions(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public void IsAuthorized_WhenOptionsAreAuthorized_ThenShouldReturnTrue()
        {
            Run(f => f.SetAuthorizedOptions(), f => f.IsAuthorized(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public void IsAuthorized_WhenOptionsAreUnauthorized_ThenShouldReturnTrue()
        {
            Run(f => f.SetUnauthorizedOptions(), f => f.IsAuthorized(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreAuthorized_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.SetAuthorizedOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsValid));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOptionsAreUnauthorized_ThenShouldReturnInvalidAuthorizationResult()
        {
            return RunAsync(f => f.SetUnauthorizedOptions(), f => f.GetAuthorizationResultAsync(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsValid.Should().BeFalse();
                r.Errors.Should().HaveCount(2).And.Contain(f.EmployerFeatureDisabledAuthorizationError).And.Contain(f.ProviderPermissionNotGrantedAuthorizationError);
            });
        }

        [Test]
        public void GetAuthorizationResult_WhenOptionsAreAuthorized_ThenShouldReturnValidAuthorizationResult()
        {
            Run(f => f.SetAuthorizedOptions(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsValid));
        }

        [Test]
        public void GetAuthorizationResult_WhenOptionsAreUnauthorized_ThenShouldReturnInvalidAuthorizationResult()
        {
            Run(f => f.SetUnauthorizedOptions(), f => f.GetAuthorizationResult(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsValid.Should().BeFalse();
                r.Errors.Should().HaveCount(2).And.Contain(f.EmployerFeatureDisabledAuthorizationError).And.Contain(f.ProviderPermissionNotGrantedAuthorizationError);
            });
        }
    }

    public class AuthorizationServiceTestsFixture
    {
        public string[] Options { get; set; }
        public IAuthorizationService AuthorizationService { get; set; }
        public Mock<IAuthorizationHandler> EmployerFeatureAuthorizationHandler { get; set; }
        public EmployerFeatureDisabledAuthorizationError EmployerFeatureDisabledAuthorizationError { get; set; }
        public Mock<IAuthorizationHandler> ProviderOperationAuthorizationHandler { get; set; }
        public ProviderPermissionNotGrantedAuthorizationError ProviderPermissionNotGrantedAuthorizationError { get; set; }

        public AuthorizationServiceTestsFixture()
        {
            Options = new []
            {
                EmployerFeatures.Transfers,
                ProviderOperations.CreateCohort
            };

            EmployerFeatureAuthorizationHandler = new Mock<IAuthorizationHandler>();
            ProviderOperationAuthorizationHandler = new Mock<IAuthorizationHandler>();

            AuthorizationService = new AuthorizationService(new List<IAuthorizationHandler>
            {
                EmployerFeatureAuthorizationHandler.Object,
                ProviderOperationAuthorizationHandler.Object
            });
        }

        public Task<bool> IsAuthorizedAsync()
        {
            return AuthorizationService.IsAuthorizedAsync(Options);
        }

        public Task<AuthorizationResult> GetAuthorizationResultAsync()
        {
            return AuthorizationService.GetAuthorizationResultAsync(Options);
        }

        public AuthorizationServiceTestsFixture SetAuthorizedOptions()
        {
            EmployerFeatureAuthorizationHandler.Setup(h => h.GetAuthorizationResultAsync(Options)).ReturnsAsync(new AuthorizationResult());
            ProviderOperationAuthorizationHandler.Setup(h => h.GetAuthorizationResultAsync(Options)).ReturnsAsync(new AuthorizationResult());

            return this;
        }

        public AuthorizationServiceTestsFixture SetUnauthorizedOptions()
        {
            EmployerFeatureDisabledAuthorizationError = new EmployerFeatureDisabledAuthorizationError();
            ProviderPermissionNotGrantedAuthorizationError = new ProviderPermissionNotGrantedAuthorizationError();

            EmployerFeatureAuthorizationHandler.Setup(h => h.GetAuthorizationResultAsync(Options)).ReturnsAsync(new AuthorizationResult(EmployerFeatureDisabledAuthorizationError));
            ProviderOperationAuthorizationHandler.Setup(h => h.GetAuthorizationResultAsync(Options)).ReturnsAsync(new AuthorizationResult(ProviderPermissionNotGrantedAuthorizationError));

            return this;
        }

        public bool IsAuthorized()
        {
            return AuthorizationService.IsAuthorized(Options);
        }

        public AuthorizationResult GetAuthorizationResult()
        {
            return AuthorizationService.GetAuthorizationResult(Options);
        }
    }
}