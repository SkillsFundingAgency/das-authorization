using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.UnitTests
{
    [TestFixture]
    public class AuthorizationResultTests : FluentTest<AuthorizationResultTestsFixture>
    {
        [Test]
        public void Constructor_WhenConstructingAnAuthorizationResult_ThenShouldConstructAValidAuthorizationResult()
        {
            Run(f => new AuthorizationResult(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeTrue();
                r.Errors.Should().BeEmpty();
            });
        }

        [Test]
        public void Constructor_WhenConstructingAnAuthorizationResultWithAnError_ThenShouldConstructAnInvalidAuthorizationResult()
        {
            Run(f => new AuthorizationResult(f.EmployerRoleNotAuthorized), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(1).And.Contain(f.EmployerRoleNotAuthorized);
            });
        }

        [Test]
        public void Constructor_WhenConstructingAnAuthorizationResultWithErrors_ThenShouldConstructAnInvalidAuthorizationResult()
        {
            Run(f => new AuthorizationResult(f.Errors), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(f.Errors.Count).And.Contain(f.Errors);
            });
        }

        [Test]
        public void AddError_WhenAddingAnError_ThenShouldInvalidateAuthorizationResult()
        {
            Run(f => new AuthorizationResult().AddError(f.EmployerRoleNotAuthorized), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(1).And.Contain(f.EmployerRoleNotAuthorized);
            });
        }

        [Test]
        public void AddError_WhenAddingAErrors_ThenShouldInvalidateAuthorizationResult()
        {
            Run(f => new AuthorizationResult().AddError(f.EmployerRoleNotAuthorized).AddError(f.ProviderPermissionNotGranted), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsAuthorized.Should().BeFalse();
                r.Errors.Should().HaveCount(f.Errors.Count).And.Contain(f.Errors);
            });
        }
    }

    public class AuthorizationResultTestsFixture
    {
        public EmployerRoleNotAuthorized EmployerRoleNotAuthorized { get; set; }
        public ProviderPermissionNotGranted ProviderPermissionNotGranted { get; set; }
        public List<AuthorizationError> Errors { get; set; }

        public AuthorizationResultTestsFixture()
        {
            EmployerRoleNotAuthorized = new EmployerRoleNotAuthorized();
            ProviderPermissionNotGranted = new ProviderPermissionNotGranted();

            Errors = new List<AuthorizationError>
            {
                EmployerRoleNotAuthorized,
                ProviderPermissionNotGranted
            };
        }
    }
}