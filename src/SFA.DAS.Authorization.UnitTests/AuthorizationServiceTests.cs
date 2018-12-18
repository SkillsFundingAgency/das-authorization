using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.ProviderPermissions;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationServiceTests : FluentTest<AuthorizationServiceTestsFixture>
    {
        [Test]
        public void Authorize_WhenOperationIsAuthorized_ThenShouldNotThrowException()
        {
            Test(f => f.SetAuthorizedOptions(), f => f.Authorize(), f => {});
        }
        
        [Test]
        public void Authorize_WhenOperationIsUnauthorized_ThenShouldThrowException()
        {
            TestException(f => f.SetUnauthorizedOptions(), f => f.Authorize(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
        
        [Test]
        public Task AuthorizeAsync_WhenOperationIsAuthorized_ThenShouldNotThrowException()
        {
            return TestAsync(f => f.SetAuthorizedOptions(), f => f.AuthorizeAsync(), f => {});
        }
        
        [Test]
        public Task AuthorizeAsync_WhenOperationIsUnauthorized_ThenShouldThrowException()
        {
            return TestExceptionAsync(f => f.SetUnauthorizedOptions(), f => f.AuthorizeAsync(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
        
        [Test]
        public Task IsAuthorizedAsync_WhenOperationIsAuthorized_ThenShouldReturnTrue()
        {
            return TestAsync(f => f.SetAuthorizedOptions(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public Task IsAuthorizedAsync_WhenOperationIsUnauthorized_ThenShouldReturnTrue()
        {
            return TestAsync(f => f.SetUnauthorizedOptions(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public void IsAuthorized_WhenOperationIsAuthorized_ThenShouldReturnTrue()
        {
            Test(f => f.SetAuthorizedOptions(), f => f.IsAuthorized(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public void IsAuthorized_WhenOperationIsUnauthorized_ThenShouldReturnTrue()
        {
            Test(f => f.SetUnauthorizedOptions(), f => f.IsAuthorized(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOperationIsAuthorized_ThenShouldReturnValidAuthorizationResult()
        {
            return TestAsync(f => f.SetAuthorizedOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOperationIsUnauthorized_ThenShouldReturnInvalidAuthorizationResult()
        {
            return TestAsync(f => f.SetUnauthorizedOptions(), f => f.GetAuthorizationResultAsync(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(3).And.Contain(f.EmployerFeatureUserNotWhitelisted).And.Contain(f.EmployerRoleNotAuthorized).And.Contain(f.ProviderPermissionNotGranted);
            });
        }

        [Test]
        public void GetAuthorizationResult_WhenOperationIsAuthorized_ThenShouldReturnValidAuthorizationResult()
        {
            Test(f => f.SetAuthorizedOptions(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public void GetAuthorizationResult_WhenOperationIsUnauthorized_ThenShouldReturnInvalidAuthorizationResult()
        {
            Test(f => f.SetUnauthorizedOptions(), f => f.GetAuthorizationResult(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(3).And.Contain(f.EmployerFeatureUserNotWhitelisted).And.Contain(f.EmployerRoleNotAuthorized).And.Contain(f.ProviderPermissionNotGranted);
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
        public EmployerFeatureUserNotWhitelisted EmployerFeatureUserNotWhitelisted { get; set; }
        public Mock<IAuthorizationHandler> EmployerRolesAuthorizationHandler { get; set; }
        public EmployerRoleNotAuthorized EmployerRoleNotAuthorized { get; set; }
        public Mock<IAuthorizationHandler> ProviderOperationAuthorizationHandler { get; set; }
        public ProviderPermissionNotGranted ProviderPermissionNotGranted { get; set; }


        public AuthorizationServiceTestsFixture()
        {
            Options = new []
            {
                EmployerFeature.ProviderRelationships,
                EmployerRole.OwnerOrTransactor,
                ProviderOperation.CreateCohort
            };

            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContext = new Mock<IAuthorizationContext>();
            EmployerFeatureAuthorizationHandler = new Mock<IAuthorizationHandler>();
            EmployerRolesAuthorizationHandler = new Mock<IAuthorizationHandler>();
            ProviderOperationAuthorizationHandler = new Mock<IAuthorizationHandler>();
            
            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(AuthorizationContext.Object);
            EmployerFeatureAuthorizationHandler.Setup(h => h.Namespace).Returns(EmployerFeature.Namespace);
            EmployerRolesAuthorizationHandler.Setup(h => h.Namespace).Returns(EmployerRole.Namespace);
            ProviderOperationAuthorizationHandler.Setup(h => h.Namespace).Returns(ProviderOperation.Namespace);

            AuthorizationService = new AuthorizationService(AuthorizationContextProvider.Object, new List<IAuthorizationHandler>
            {
                EmployerFeatureAuthorizationHandler.Object,
                EmployerRolesAuthorizationHandler.Object,
                ProviderOperationAuthorizationHandler.Object
            });
        }

        public void Authorize()
        {
            AuthorizationService.Authorize(Options);
        }

        public Task AuthorizeAsync()
        {
            return AuthorizationService.AuthorizeAsync(Options);
        }

        public bool IsAuthorized()
        {
            return AuthorizationService.IsAuthorized(Options);
        }

        public Task<bool> IsAuthorizedAsync()
        {
            return AuthorizationService.IsAuthorizedAsync(Options);
        }

        public AuthorizationResult GetAuthorizationResult()
        {
            return AuthorizationService.GetAuthorizationResult(Options);
        }

        public Task<AuthorizationResult> GetAuthorizationResultAsync()
        {
            return AuthorizationService.GetAuthorizationResultAsync(Options);
        }

        public AuthorizationServiceTestsFixture SetAuthorizedOptions()
        {
            EmployerFeatureAuthorizationHandler.Setup(h => h.GetAuthorizationResult(
                    new [] { EmployerFeature.ProviderRelationshipsOption }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult());
            
            EmployerRolesAuthorizationHandler.Setup(h => h.GetAuthorizationResult(
                    new [] { EmployerRole.OwnerOption + "," + EmployerRole.TransactorOption }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult());

            ProviderOperationAuthorizationHandler.Setup(h => h.GetAuthorizationResult(
                    new [] { ProviderOperation.CreateCohortOption }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult());

            return this;
        }

        public AuthorizationServiceTestsFixture SetUnauthorizedOptions()
        {
            EmployerFeatureUserNotWhitelisted = new EmployerFeatureUserNotWhitelisted();
            EmployerRoleNotAuthorized = new EmployerRoleNotAuthorized();
            ProviderPermissionNotGranted = new ProviderPermissionNotGranted();

            EmployerFeatureAuthorizationHandler.Setup(h => h.GetAuthorizationResult(
                    new [] { EmployerFeature.ProviderRelationshipsOption }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult(EmployerFeatureUserNotWhitelisted));

            EmployerRolesAuthorizationHandler.Setup(h => h.GetAuthorizationResult(
                    new [] { EmployerRole.OwnerOption + "," + EmployerRole.TransactorOption }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult(EmployerRoleNotAuthorized));
            
            ProviderOperationAuthorizationHandler.Setup(h => h.GetAuthorizationResult(
                    new [] { ProviderOperation.CreateCohortOption }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult(ProviderPermissionNotGranted));

            return this;
        }
    }
}