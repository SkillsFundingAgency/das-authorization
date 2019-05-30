using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.Authorization.Cache;

namespace SFA.DAS.Authorization.UnitTests.Cache
{
    [TestFixture]
    public class AuthorizationCacheServiceTests
    {
        [Test]
        public void Constructor_Valid_ShouldNotThrowException()
        {
            var fixtures = new AuthorizationCacheServiceTestFixtures();

            fixtures.CreateService();

            Assert.Pass("Constructed service without getting an exception");
        }

        [TestCase(true)]
        [TestCase(false)]
        public Task GetOrAdd_WithANonCachedResult_ShouldReturnResultFromAuthHandler(bool isAuthorized)
        {
            var fixtures = new AuthorizationCacheServiceTestFixtures()
                .AddHandler(out var handlerMock)
                .AddHandlerConfig(handlerMock, "ABC")
                .WithAuthorizationResult(handlerMock, "ABC", isAuthorized, out var authorizationResult)
                .NotInCache(handlerMock, "ABC", authorizationResult);

            return fixtures.AssertGetOrAdd(handlerMock, isAuthorized);
        }

        [Test]
        public Task GetOrAdd_WithAnyValidReequest_ShouldQueryCache()
        {
            var fixtures = new AuthorizationCacheServiceTestFixtures()
                .AddHandler(out var handlerMock)
                .AddHandlerConfig(handlerMock, "ABC")
                .WithAuthorizationResult(handlerMock, "ABC", out var authorizationResult)
                .NotInCache(handlerMock, "ABC", authorizationResult);

            return fixtures.VerifyCacheQueriedWithKey(handlerMock, "ABC", Times.Once());
        }

        [Test]
        public Task GetOrAdd_WithACachedResult_ShouldNotCallHandler()
        {
            var fixtures = new AuthorizationCacheServiceTestFixtures()
                .AddHandler(out var handlerMock)
                .AddHandlerConfig(handlerMock, "ABC")
                .WithAuthorizationResult(handlerMock, "ABC", out var authorizationResult)
                .InCache(handlerMock, "ABC", authorizationResult);

            return fixtures.VerifyHandlerCalledWithKey(handlerMock, "ABC", Times.Never());
        }


        [Test]
        public Task GetOrAdd_WithANonCachedResult_ShouldCallHandler()
        {
            var fixtures = new AuthorizationCacheServiceTestFixtures()
                .AddHandler(out var handlerMock)
                .AddHandlerConfig(handlerMock, "ABC")
                .WithAuthorizationResult(handlerMock, "ABC", out var authorizationResult)
                .NotInCache(handlerMock, "ABC", authorizationResult);

            return fixtures.VerifyHandlerCalledWithKey(handlerMock, "ABC", Times.Once());
        }

        [Test]
        public Task GetOrAdd_WithANonCachedResult_ShouldWriteResultToCache()
        {
            var fixtures = new AuthorizationCacheServiceTestFixtures()
                .AddHandler(out var handlerMock)
                .AddHandlerConfig(handlerMock, "ABC")
                .WithAuthorizationResult(handlerMock, "ABC", out var authorizationResult)
                .NotInCache(handlerMock, "ABC", authorizationResult);

            return fixtures.VerifyCacheUpdatedWithKey(handlerMock, "ABC", Times.Once());
        }

        [Test]
        public Task GetOrAdd_WithACachedResult_ShouldNotRewriteResultToCache()
        {
            var fixtures = new AuthorizationCacheServiceTestFixtures()
                .AddHandler(out var handlerMock)
                .AddHandlerConfig(handlerMock, "ABC")
                .WithAuthorizationResult(handlerMock, "ABC", out var authorizationResult)
                .InCache(handlerMock, "ABC", authorizationResult);

            return fixtures.VerifyCacheUpdatedWithKey(handlerMock, "ABC", Times.Never());
        }
    }

    public class AuthorizationCacheServiceTestFixtures
    {
        private readonly List<Mock<IAuthorizationHandler>> _authorizationHandlers;
        private readonly List<Mock<IAuthorizationHandlerCacheConfig>> _authorizationHandlerCache;

        public AuthorizationCacheServiceTestFixtures()
        {
            MemoryCacheMock = new Mock<IMemoryCache>();

            MemoryCacheMock
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>());

            AuthorizationContextMock = new Mock<IAuthorizationContext>();

            _authorizationHandlers = new List<Mock<IAuthorizationHandler>>();
            _authorizationHandlerCache = new List<Mock<IAuthorizationHandlerCacheConfig>>();
        }

        public Mock<IMemoryCache> MemoryCacheMock { get;  }
        public IMemoryCache MemoryCache => MemoryCacheMock.Object;

        public IReadOnlyCollection<Mock<IAuthorizationHandlerCacheConfig>> AuthorizationHandlerCacheConfigsMock => _authorizationHandlerCache;

        public IAuthorizationHandlerCacheConfig[] AuthorizationHandlerCacheConfig => AuthorizationHandlerCacheConfigsMock.Select(m => m.Object).ToArray();

        public Mock<IAuthorizationContext> AuthorizationContextMock { get; }
        public IAuthorizationContext AuthorizationContext => AuthorizationContextMock.Object;

        public AuthorizationCacheServiceTestFixtures AddHandler(out Mock<IAuthorizationHandler> mockHandler)
        {
            mockHandler = new Mock<IAuthorizationHandler>();
            _authorizationHandlers.Add(mockHandler);
            return this;
        }

        public Mock<IAuthorizationHandler>[] LastHandler => new[]{_authorizationHandlers.Last()};

        public AuthorizationCacheServiceTestFixtures AddHandlerConfig(Mock<IAuthorizationHandler> handler, object key)
        {
            return AddHandlerConfig(fixtures => new[] {handler}, key);
        }

        public AuthorizationCacheServiceTestFixtures AddHandlerConfig(Func<AuthorizationCacheServiceTestFixtures, Mock<IAuthorizationHandler>[]> getHandler, object key)
        {
            var mockConfig = new Mock<IAuthorizationHandlerCacheConfig>();
            mockConfig
                .Setup(c => c.SupportsHandlerTypes)
                .Returns(() => getHandler(this).Select(hm => hm.Object.GetType()).ToArray());

            mockConfig
                .Setup(c => c.GetAuthorizationKey(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<IAuthorizationContext>()))
                .Returns(key);

            _authorizationHandlerCache.Add(mockConfig);

            return this;
        }

        public AuthorizationCacheServiceTestFixtures WithAuthorizationResult(Mock<IAuthorizationHandler> handlerMock, object key, out AuthorizationResult authorizationResult)
        {
            return WithAuthorizationResult(handlerMock, key, true, out authorizationResult);
        }

        public AuthorizationCacheServiceTestFixtures WithAuthorizationResult(Mock<IAuthorizationHandler> handlerMock, object key, bool authorize, out AuthorizationResult authorizationResult)
        {
            authorizationResult = new AuthorizationResult();
            if (!authorize)
            {
                authorizationResult.AddError(new TestAuthorizationError());
            }

            return this;
        }

        public AuthorizationCacheServiceTestFixtures InCache(Mock<IAuthorizationHandler> handlerMock, object key, AuthorizationResult authorizationResult)
        {
            object cachedItem = authorizationResult;

            MemoryCacheMock
                .Setup(mcm => mcm.TryGetValue(key, out cachedItem))
                .Returns(true);

            return this;
        }

        public AuthorizationCacheServiceTestFixtures NotInCache(Mock<IAuthorizationHandler> handlerMock, object key, AuthorizationResult authorizationResult)
        {
            handlerMock
                .Setup(mh => mh.GetAuthorizationResult(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<IAuthorizationContext>()))
                .ReturnsAsync(authorizationResult);

            return this;
        }

        public AuthorizationCacheService CreateService()
        {
            return new AuthorizationCacheService(MemoryCache, AuthorizationHandlerCacheConfig);
        }

        public async Task AssertGetOrAdd(Mock<IAuthorizationHandler> handlerMock, bool expectAuthorized)
        {
            var svc = CreateService();
            var authorizationResult = await svc.GetOrAdd(handlerMock.Object, new List<string>(), AuthorizationContext);

            Assert.AreEqual(expectAuthorized, authorizationResult.IsAuthorized);
        }

        public async Task VerifyCacheQueriedWithKey(Mock<IAuthorizationHandler> handlerMock, object key, Times times)
        {
            var svc = CreateService();
            var authorizationResult = await svc.GetOrAdd(handlerMock.Object, new List<string>(), AuthorizationContext);

            object value = null;
            MemoryCacheMock.Verify(mc => mc.TryGetValue(key, out value), times);
        }

        public async Task VerifyCacheUpdatedWithKey(Mock<IAuthorizationHandler> handlerMock, object key, Times times)
        {
            var svc = CreateService();
            var authorizationResult = await svc.GetOrAdd(handlerMock.Object, new List<string>(), AuthorizationContext);

            MemoryCacheMock.Verify(mc => mc.CreateEntry(key), times);
        }

        public async Task VerifyHandlerCalledWithKey(Mock<IAuthorizationHandler> handlerMock, object key, Times times)
        {
            var svc = CreateService();
            var authorizationResult = await svc.GetOrAdd(handlerMock.Object, new List<string>(), AuthorizationContext);

            handlerMock.Verify(h => h.GetAuthorizationResult(It.IsAny<IReadOnlyCollection<string>>(), AuthorizationContext), times);
        }
    }

    // need this because the base class is abstract and doesn't have a parameterless constructor
    class TestAuthorizationError : AuthorizationError
    {
        public TestAuthorizationError() : base("Test authorization error")
        {

        }
    }
}
