using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.EmployerRoles.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationHandlerTests : FluentTest<EmployerRolesAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndOptionsAreNotAvailable_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenAndedOptionsAreAvailable_ThenShouldThrowNotImplementedException()
        {
            return RunAsync(f => f.SetAndedOptions(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<NotImplementedException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndValidSingleEmployerRoleOptionsAreAvailableAndEmployerRoleContextIsNotAvailable_ThenShouldThrowAuthorizationContextKeyNotFoundException()
        {
            return RunAsync(f => f.SetValidSingleEmployerRolesOptions(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndValidSingleEmployerRoleOptionsAreAvailableAndEmployerRoleContextIsValid_ThenShouldReturnValidAuthorizationResult()
        {
            return RunAsync(f =>f.SetValidSingleEmployerRolesOptions().SetAuthorizationContextValues().SetHasRole(true),
                f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndValidSingleEmployerRoleOptionsAreAvailableAndEmployerRoleContextIsValidAndHasRoleReturnsFalse_ThenShouldReturnErrorInAuthorizationResult()
        {
            return RunAsync(f => f.SetValidSingleEmployerRolesOptions().SetAuthorizationContextValues().SetHasRole(false),
                f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized == false && r2.Errors.Count() == 1 && r2.HasError<EmployerRoleNotAuthorized>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndValidSingleEmployerRoleOptionsAreAvailableAndEmployerRoleContextIsValid_ThenShouldCallApiCorrectly()
        {
            return RunAsync(f => f.SetValidSingleEmployerRolesOptions().SetAuthorizationContextValues(), 
                f => f.GetAuthorizationResult(), f => f.VerifyHasRole());
        }

        [Test]
        public Task GetAuthorizationResult_WhenGettingAuthorizationResultAndValidOredEmployerRoleOptionsAreAvailableAndEmployerRoleContextIsValid_ThenShouldCallApiCorrectly()
        {
            return RunAsync(f => f.SetValidOredEmployerRolesOptions().SetAuthorizationContextValues().SetHasRole(true),
                f => f.GetAuthorizationResult(), f => f.VerifyHasRole());
        }
    }

    public class EmployerRolesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public List<Role> ExpectedRoles { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public Mock<IEmployerRolesApiClientDummy> MockEmployerRolesApiClient { get; set; }
        public AuthorizationHandler Handler { get; set; }

        public const long AccountId = 112L;
        public readonly Guid UserRef = Guid.NewGuid();

        public EmployerRolesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            ExpectedRoles = new List<Role>();
            AuthorizationContext = new AuthorizationContext();
            MockEmployerRolesApiClient = new Mock<IEmployerRolesApiClientDummy>();
            Handler = new AuthorizationHandler(MockEmployerRolesApiClient.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetValidSingleEmployerRolesOptions()
        {
            Options.AddRange(new [] { EmployerRole.Owner });
            ExpectedRoles.Add(Role.Owner);

            return this;
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetValidOredEmployerRolesOptions()
        {
            Options.AddRange(new[] { EmployerRole.Owner + "," + EmployerRole.Transactor });
            ExpectedRoles.Add(Role.Owner);
            ExpectedRoles.Add(Role.Transactor);

            return this;
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetAndedOptions()
        {
            Options.AddRange(new[] { EmployerRole.Any, EmployerRole.Owner });

            return this;
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetAuthorizationContextValues(long? accountId = AccountId)
        {
            AuthorizationContext.AddEmployerRoleValues(accountId, UserRef);

            return this;
        }

        public EmployerRolesAuthorizationHandlerTestsFixture SetHasRole(bool result)
        {
            var roles = Options.Single().Split(',').Select(x => Enum.Parse<Role>(x.Split('.').Last()));

            MockEmployerRolesApiClient.Setup(c => c.HasRole(
                    It.Is<RoleRequest>(r =>
                        r.UserRef == UserRef &&
                        r.EmployerAccountId == AccountId &&
                        r.Roles.SequenceEqual(roles))))
                .ReturnsAsync(result);

            return this;
        }

        public void VerifyHasRole()
        {
            MockEmployerRolesApiClient.Verify(c => c.HasRole(
                It.Is<RoleRequest>(r =>
                    r.UserRef == UserRef &&
                    r.EmployerAccountId == AccountId &&
                    r.Roles.Length == ExpectedRoles.Count &&
                    r.Roles.All(role => ExpectedRoles.Any(expectedRole => expectedRole == role)))));
        }
    }
}