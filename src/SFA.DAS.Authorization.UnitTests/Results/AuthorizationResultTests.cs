using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Authorization.EmployerUserRoles.Errors;
using SFA.DAS.Authorization.Errors;
using SFA.DAS.Authorization.ProviderPermissions.Errors;
using SFA.DAS.Authorization.Results;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.UnitTests.Results
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationResultTests : FluentTest<AuthorizationResultTestsFixture>
    {
        [Test]
        public void Constructor_WhenConstructingAnAuthorizationResult_ThenShouldConstructAValidAuthorizationResult()
        {
            Test(f => new AuthorizationResult(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeTrue();
                r.Errors.Should().BeEmpty();
            });
        }

        [Test]
        public void Constructor_WhenConstructingAnAuthorizationResultWithAnError_ThenShouldConstructAnInvalidAuthorizationResult()
        {
            Test(f => new AuthorizationResult(f.EmployerUserRoleNotAuthorized), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(1).And.Contain(f.EmployerUserRoleNotAuthorized);
            });
        }

        [Test]
        public void Constructor_WhenConstructingAnAuthorizationResultWithErrors_ThenShouldConstructAnInvalidAuthorizationResult()
        {
            Test(f => new AuthorizationResult(f.Errors), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(f.Errors.Count).And.Contain(f.Errors);
            });
        }

        [Test]
        public void AddError_WhenAddingAnError_ThenShouldInvalidateAuthorizationResult()
        {
            Test(f => new AuthorizationResult().AddError(f.EmployerUserRoleNotAuthorized), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(1).And.Contain(f.EmployerUserRoleNotAuthorized);
            });
        }

        [Test]
        public void AddError_WhenAddingAErrors_ThenShouldInvalidateAuthorizationResult()
        {
            Test(f => new AuthorizationResult().AddError(f.EmployerUserRoleNotAuthorized).AddError(f.ProviderPermissionNotGranted), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(f.Errors.Count).And.Contain(f.Errors);
            });
        }
        
        [Test]
        public void HasError_WhenAnErrorOfTypeExists_ThenShouldReturnTrue()
        {
            Test(f => new AuthorizationResult().AddError(f.EmployerUserRoleNotAuthorized).HasError<EmployerUserRoleNotAuthorized>(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public void HasError_WhenAnErrorOfTypeDoesNotExist_ThenShouldReturnFalse()
        {
            Test(f => new AuthorizationResult().AddError(f.EmployerUserRoleNotAuthorized).HasError<ProviderPermissionNotGranted>(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public void ToString_WhenAuthorized_ThenShouldReturnAuthorizedDescription()
        {
            Test(f => new AuthorizationResult().ToString(), (f, r) => r.Should().Be($"IsAuthorized: True, Errors: None"));
        }

        [Test]
        public void ToString_WhenUnauthorized_ThenShouldReturnUnauthorizedDescription()
        {
            Test(f => new AuthorizationResult().AddError(f.EmployerUserRoleNotAuthorized).AddError(f.ProviderPermissionNotGranted).ToString(), (f, r) => r.Should().Be($"IsAuthorized: False, Errors: {f.EmployerUserRoleNotAuthorized.Message}, {f.ProviderPermissionNotGranted.Message}"));
        }

        [Test]
        public void HasAnyErrors_WhenHasError_ThenShouldReturnTrue()
        {
            Test(f => new AuthorizationResult().AddError(f.EmployerUserRoleNotAuthorized), (f, r) => r.Should().Match<AuthorizationResult>(r2 => r2.HasAnyError));
        }

        [Test]
        public void HasAnyErrors_WhenHasNoError_ThenShouldReturnFalse()
        {
            Test(f => new AuthorizationResult(), (f, r) => r.Should().Match<AuthorizationResult>(r2 => !r2.HasAnyError));
        }
    }

    public class AuthorizationResultTestsFixture
    {
        public EmployerUserRoleNotAuthorized EmployerUserRoleNotAuthorized { get; set; }
        public ProviderPermissionNotGranted ProviderPermissionNotGranted { get; set; }
        public List<AuthorizationError> Errors { get; set; }

        public AuthorizationResultTestsFixture()
        {
            EmployerUserRoleNotAuthorized = new EmployerUserRoleNotAuthorized();
            ProviderPermissionNotGranted = new ProviderPermissionNotGranted();

            Errors = new List<AuthorizationError>
            {
                EmployerUserRoleNotAuthorized,
                ProviderPermissionNotGranted
            };
        }
    }
}