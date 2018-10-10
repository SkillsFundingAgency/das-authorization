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
        public Task IsAuthorizedAsync_WhenOperationIsAuthorized_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.SetAuthorizedOptions(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public Task IsAuthorizedAsync_WhenOperationIsUnauthorized_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.SetUnauthorizedOptions(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public void IsAuthorized_WhenOperationIsAuthorized_ThenShouldReturnTrue()
        {
            Run(f => f.SetAuthorizedOptions(), f => f.IsAuthorized(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public void IsAuthorized_WhenOperationIsUnauthorized_ThenShouldReturnTrue()
        {
            Run(f => f.SetUnauthorizedOptions(), f => f.IsAuthorized(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOperationIsAuthorized_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.SetAuthorizedOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOperationIsUnauthorized_ThenShouldReturnInvalidAuthorizationResult()
        {
            return RunAsync(f => f.SetUnauthorizedOptions(), f => f.GetAuthorizationResultAsync(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(2).And.Contain(f.EmployerRoleNotAuthorized).And.Contain(f.ProviderPermissionNotGranted);
            });
        }

        [Test]
        public void GetAuthorizationResult_WhenOperationIsAuthorized_ThenShouldReturnValidAuthorizationResult()
        {
            Run(f => f.SetAuthorizedOptions(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public void GetAuthorizationResult_WhenOperationIsUnauthorized_ThenShouldReturnInvalidAuthorizationResult()
        {
            Run(f => f.SetUnauthorizedOptions(), f => f.GetAuthorizationResult(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(2).And.Contain(f.EmployerRoleNotAuthorized).And.Contain(f.ProviderPermissionNotGranted);
            });
        }
    }

    public class AuthorizationServiceTestsFixture
    {
        public string[] Options { get; set; }
        public Mock<IAuthorizationContextProvider> AuthorizationContextProvider { get; set; }
        public Mock<IAuthorizationContext> AuthorizationContext { get; set; }
        public IAuthorizationService AuthorizationService { get; set; }
        public Mock<IAuthorizationHandler> EmployerFeatureAuthorizationHandler { get; set; }
        public EmployerRoleNotAuthorized EmployerRoleNotAuthorized { get; set; }
        public Mock<IAuthorizationHandler> ProviderOperationAuthorizationHandler { get; set; }
        public ProviderPermissionNotGranted ProviderPermissionNotGranted { get; set; }

        public AuthorizationServiceTestsFixture()
        {
            Options = new []
            {
                EmployerFeatures.Transfers,
                ProviderPermissions.CreateCohort
            };

            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContext = new Mock<IAuthorizationContext>();
            EmployerFeatureAuthorizationHandler = new Mock<IAuthorizationHandler>();
            ProviderOperationAuthorizationHandler = new Mock<IAuthorizationHandler>();

            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(AuthorizationContext.Object);

            AuthorizationService = new AuthorizationService(AuthorizationContextProvider.Object, new List<IAuthorizationHandler>
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
            EmployerFeatureAuthorizationHandler.Setup(h => h.GetAuthorizationResultAsync(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult());
            ProviderOperationAuthorizationHandler.Setup(h => h.GetAuthorizationResultAsync(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult());

            return this;
        }

        public AuthorizationServiceTestsFixture SetUnauthorizedOptions()
        {
            EmployerRoleNotAuthorized = new EmployerRoleNotAuthorized();
            ProviderPermissionNotGranted = new ProviderPermissionNotGranted();

            EmployerFeatureAuthorizationHandler.Setup(h => h.GetAuthorizationResultAsync(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult(EmployerRoleNotAuthorized));
            ProviderOperationAuthorizationHandler.Setup(h => h.GetAuthorizationResultAsync(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult(ProviderPermissionNotGranted));

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