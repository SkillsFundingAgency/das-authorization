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
                r.IsValid.Should().BeTrue();
                r.Errors.Should().BeEmpty();
            });
        }

        [Test]
        public void Constructor_WhenConstructingAnAuthorizationResultWithAnError_ThenShouldConstructAnInvalidAuthorizationResult()
        {
            Run(f => new AuthorizationResult(f.EmployerFeatureDisabledAuthorizationError), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsValid.Should().BeFalse();
                r.Errors.Should().HaveCount(1).And.Contain(f.EmployerFeatureDisabledAuthorizationError);
            });
        }

        [Test]
        public void Constructor_WhenConstructingAnAuthorizationResultWithErrors_ThenShouldConstructAnInvalidAuthorizationResult()
        {
            Run(f => new AuthorizationResult(f.Errors), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsValid.Should().BeFalse();
                r.Errors.Should().HaveCount(f.Errors.Count).And.Contain(f.Errors);
            });
        }

        [Test]
        public void AddError_WhenAddingAnError_ThenShouldInvalidateAuthorizationResult()
        {
            Run(f => new AuthorizationResult().AddError(f.EmployerFeatureDisabledAuthorizationError), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsValid.Should().BeFalse();
                r.Errors.Should().HaveCount(1).And.Contain(f.EmployerFeatureDisabledAuthorizationError);
            });
        }

        [Test]
        public void AddError_WhenAddingAErrors_ThenShouldInvalidateAuthorizationResult()
        {
            Run(f => new AuthorizationResult().AddError(f.EmployerFeatureDisabledAuthorizationError).AddError(f.ProviderPermissionNotGrantedAuthorizationError), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsValid.Should().BeFalse();
                r.Errors.Should().HaveCount(f.Errors.Count).And.Contain(f.Errors);
            });
        }
    }

    public class AuthorizationResultTestsFixture
    {
        public EmployerFeatureDisabledAuthorizationError EmployerFeatureDisabledAuthorizationError { get; set; }
        public ProviderPermissionNotGrantedAuthorizationError ProviderPermissionNotGrantedAuthorizationError { get; set; }
        public List<AuthorizationError> Errors { get; set; }

        public AuthorizationResultTestsFixture()
        {
            EmployerFeatureDisabledAuthorizationError = new EmployerFeatureDisabledAuthorizationError();
            ProviderPermissionNotGrantedAuthorizationError = new ProviderPermissionNotGrantedAuthorizationError();

            Errors = new List<AuthorizationError>
            {
                EmployerFeatureDisabledAuthorizationError,
                ProviderPermissionNotGrantedAuthorizationError
            };
        }
    }
}