using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Cache;

namespace SFA.DAS.Authorization.UnitTests.Cache
{
    [TestFixture]
    public class AuthorizationHandlerCacheDecoratorTests
    {
        [Test]
        public void Prefix_WithWrappedHandler_ShouldPassthroughPrefix()
        {
            const string expectedPrefix = "ABC";

            var fixtures = new AuthorizationHandlerCacheDecoratorTestFixtures()
                                .WithPrefix(expectedPrefix);

            var sut = fixtures.CreateDecorator();

            // Act
            var actualResult = sut.Prefix;

            // Assert
            Assert.AreEqual(expectedPrefix, actualResult);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Prefix_WithWrappedHandler_ShouldPassthroughIsValid(bool isAuthorized)
        {
            var fixtures = new AuthorizationHandlerCacheDecoratorTestFixtures();

            // Act
            var actualResult = await fixtures.CheckAuthorization("ABC", new List<string>(), isAuthorized);

            // Assert
            Assert.AreEqual(isAuthorized, actualResult.IsAuthorized);
        }

        [Test]
        public async Task Prefix_WithWrappedHandler_ShouldPassthroughSuppliedValues()
        {
            var options = new List<string>();

            var fixtures = new AuthorizationHandlerCacheDecoratorTestFixtures();

            // Act
            await fixtures.CheckAuthorization("ABC", options, true);

            // Assert
            fixtures.AuthorizationCacheServiceMock.Verify(
                acs => acs.GetOrAdd(fixtures.AuthorizationHandler, options, fixtures.AuthorizationContext), Times.Once);
        }
    }

    class AuthorizationHandlerCacheDecoratorTestFixtures
    {
        public AuthorizationHandlerCacheDecoratorTestFixtures()
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

        public AuthorizationHandlerCacheDecoratorTestFixtures WithPrefix(string prefix)
        {
            AuthorizationHandlerMock
                .Setup(ah => ah.Prefix)
                .Returns(prefix);

            return this;
        }

        public AuthorizationHandlerCacheDecoratorTestFixtures WithExpectedAuthorizationResponse(IReadOnlyCollection<string> options, bool asAuthorized)
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

        public AuthorizationHandlerCacheDecorator CreateDecorator()
        {
            return new AuthorizationHandlerCacheDecorator(AuthorizationCacheService, AuthorizationHandler);
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
