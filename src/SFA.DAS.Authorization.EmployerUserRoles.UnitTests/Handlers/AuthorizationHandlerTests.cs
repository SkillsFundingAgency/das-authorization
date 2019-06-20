using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.EmployerUserRoles.Context;
using SFA.DAS.Authorization.EmployerUserRoles.Errors;
using SFA.DAS.Authorization.EmployerUserRoles.Handlers;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Results;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerAccounts.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.EmployerUserRoles.UnitTests.Handlers
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationHandlerTests : FluentTest<EmployerUserRolesAuthorizationHandlerTestsFixture>
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
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingAccountId_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextValuesMissingAccountId(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsMissingUserRef_ThenShouldThrowKeyNotFoundException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextValuesMissingUserRef(), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<KeyNotFoundException>());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableButContainsInvalidAccountId_ThenShouldThrowInvalidOperationException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextValues(null, Guid.NewGuid()), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndAuthorizationContextIsAvailableButContainsInvalidUserRef_ThenShouldThrowInvalidOperationException()
        {
            return TestExceptionAsync(f => f.SetOption().SetAuthorizationContextValues(1L, null), f => f.GetAuthorizationResult(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndUserIsInRole_ThenShouldReturnValidAuthorizationResult()
        {
            return TestAsync(f =>f.SetOption().SetAuthorizationContextValues().SetUserIsInRole(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenOptionsAreAvailableAndContextIsAvailableAndUserIsNotInRole_ThenShouldReturnErrorInAuthorizationResult()
        {
            return TestAsync(f => f.SetOption().SetAuthorizationContextValues().SetUserIsInRole(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized == false && r2.Errors.Count() == 1 && r2.HasError<EmployerUserRoleNotAuthorized>()));
        }

        [Test]
        public Task GetAuthorizationResult_WhenAnyOptionIsAvailableAndContextIsAvailableAndUserIsInAnyRole_ThenShouldReturnValidAuthorizationResult()
        {
            return TestAsync(f =>f.SetAnyOption().SetAuthorizationContextValues().SetUserIsInAnyRole(true), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public Task GetAuthorizationResult_WhenAnyOptionIsAvailableAndContextIsAvailableAndUserIsNotInAnyRole_ThenShouldReturnErrorInAuthorizationResult()
        {
            return TestAsync(f => f.SetAnyOption().SetAuthorizationContextValues().SetUserIsInAnyRole(false), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull()
                .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized == false && r2.Errors.Count() == 1 && r2.HasError<EmployerUserRoleNotAuthorized>()));
        }
    }

    public class EmployerUserRolesAuthorizationHandlerTestsFixture
    {
        public List<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public Mock<IEmployerAccountsApiClient> EmployerAccountsApiClient { get; set; }
        public AuthorizationHandler Handler { get; set; }
        public Mock<ILogger<AuthorizationHandler>> Logger { get; set; }

        public const long AccountId = 112L;
        public static readonly Guid UserRef = Guid.NewGuid();

        public EmployerUserRolesAuthorizationHandlerTestsFixture()
        {
            Options = new List<string>();
            AuthorizationContext = new AuthorizationContext();
            EmployerAccountsApiClient = new Mock<IEmployerAccountsApiClient>();
            Logger = new Mock<ILogger<AuthorizationHandler>>();
            Handler = new AuthorizationHandler(EmployerAccountsApiClient.Object, Logger.Object);
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return Handler.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetAndedOptions()
        {
            Options.AddRange(new[] { EmployerUserRole.OwnerOption, EmployerUserRole.TransactorOption });

            return this;
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetOption()
        {
            Options.AddRange(new[] { EmployerUserRole.OwnerOption + "," + EmployerUserRole.TransactorOption });

            return this;
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetAnyOption()
        {
            Options.AddRange(new[] { EmployerUserRole.AnyOption });

            return this;
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetAuthorizationContextValues()
        {
            AuthorizationContext.AddEmployerUserRoleValues(AccountId, UserRef);

            return this;
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetAuthorizationContextValues(long? accountId, Guid? userRef)
        {
            AuthorizationContext.AddEmployerUserRoleValues(accountId, userRef);

            return this;
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetAuthorizationContextValuesMissingAccountId()
        {
            AuthorizationContext.Set(AuthorizationContextKey.UserRef, UserRef);

            return this;
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetAuthorizationContextValuesMissingUserRef()
        {
            AuthorizationContext.Set(AuthorizationContextKey.AccountId, AccountId);

            return this;
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetUserIsInRole(bool result)
        {
            var roles = Options.Single().Split(',').Select(x => (UserRole)Enum.Parse(typeof(UserRole), x.Split('.').Last()));

            EmployerAccountsApiClient.Setup(c => c.IsUserInRole(
                    It.Is<IsUserInRoleRequest>(r =>
                        r.AccountId == AccountId &&
                        r.UserRef == UserRef &&
                        r.Roles.SequenceEqual(roles)),
                    CancellationToken.None))
                .ReturnsAsync(result);

            return this;
        }

        public EmployerUserRolesAuthorizationHandlerTestsFixture SetUserIsInAnyRole(bool result)
        {
            EmployerAccountsApiClient.Setup(c => c.IsUserInAnyRole(
                    It.Is<IsUserInAnyRoleRequest>(r =>
                        r.AccountId == AccountId &&
                        r.UserRef == UserRef),
                    CancellationToken.None))
                .ReturnsAsync(result);

            return this;
        }
    }
}