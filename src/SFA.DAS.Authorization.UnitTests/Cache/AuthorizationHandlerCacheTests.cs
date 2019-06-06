﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Cache;

namespace SFA.DAS.Authorization.UnitTests.Cache
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class AuthorizationHandlerCacheTests
    {
        [Test]
        public void Prefix_WhenCalled_ThenShouldPassthroughToWrappedPrefix()
        {
            const string expectedPrefix = "ABC";

            var fixtures = new AuthorizationHandlerCacheTestFixtures()
                                .WithPrefix(expectedPrefix);

            var sut = fixtures.CreateDecorator();

            // Act
            var actualResult = sut.Prefix;

            // Assert
            Assert.AreEqual(expectedPrefix, actualResult);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Prefix_WhenCalled_ThenShouldPassthroughIsValid(bool isAuthorized)
        {
            var fixtures = new AuthorizationHandlerCacheTestFixtures();

            // Act
            var actualResult = await fixtures.CheckAuthorization("ABC", new List<string>(), isAuthorized);

            // Assert
            Assert.AreEqual(isAuthorized, actualResult.IsAuthorized);
        }

        [Test]
        public async Task Prefix_WhenCalled_ThenShouldPassthroughSuppliedValues()
        {
            var options = new List<string>();

            var fixtures = new AuthorizationHandlerCacheTestFixtures();

            // Act
            await fixtures.CheckAuthorization("ABC", options, true);

            // Assert
            fixtures.AuthorizationCacheServiceMock.Verify(
                acs => acs.GetOrAdd(fixtures.AuthorizationHandler, options, fixtures.AuthorizationContext), Times.Once);
        }
    }

    class AuthorizationHandlerCacheTestFixtures
    {
        public AuthorizationHandlerCacheTestFixtures()
        {
            AuthorizationCacheServiceMock = new Mock<IAuthorizationCacheService>();
            AuthorizationHandlerMock = new Mock<IAuthorizationHandler>();
            AuthorizationContextMock = new Mock<IAuthorizationContext>();
        }

        public Mock<IAuthorizationCacheService> AuthorizationCacheServiceMock { get; }
        public IAuthorizationCacheService AuthorizationCacheService => AuthorizationCacheServiceMock.Object;

        public Mock<IAuthorizationHandler> AuthorizationHandlerMock { get; }
        public IAuthorizationHandler AuthorizationHandler => AuthorizationHandlerMock.Object;

        public Mock<IAuthorizationContext> AuthorizationContextMock { get; }
        public IAuthorizationContext AuthorizationContext => AuthorizationContextMock.Object;

        public AuthorizationHandlerCacheTestFixtures WithPrefix(string prefix)
        {
            AuthorizationHandlerMock
                .Setup(ah => ah.Prefix)
                .Returns(prefix);

            return this;
        }

        public AuthorizationHandlerCacheTestFixtures WithExpectedAuthorizationResponse(IReadOnlyCollection<string> options, bool asAuthorized)
        {
            var expectedResult = new AuthorizationResult();
            if (!asAuthorized)
            {
                expectedResult.AddError(new TestAuthorizationError());
            }

            AuthorizationCacheServiceMock
                .Setup(acs => acs.GetOrAdd(AuthorizationHandler, options, AuthorizationContext))
                .ReturnsAsync(expectedResult);

            return this;
        }

        public AuthorizationHandlerCache CreateDecorator()
        {
            return new AuthorizationHandlerCache(AuthorizationCacheService, AuthorizationHandler);
        }

        public Task<AuthorizationResult> CheckAuthorization(string prefix, IReadOnlyCollection<string> options, bool asAuthorized)
        {
            WithExpectedAuthorizationResponse(options, asAuthorized)
                .WithPrefix(prefix);

            var sut = CreateDecorator();

            return sut.GetAuthorizationResult(options, AuthorizationContext);
        }
    }
}