using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Authorization.CommitmentPermissions.Cache
{
    [TestFixture]
    public class AuthorizationContextCacheKeyProviderTests
    {
        [Test]
        public void SupportedHandlerTypes_SupportsExpectedType()
        {
            var sut = new AuthorizationContextCacheKeyProviderTestFixtures()
                .CreateSut();

            Assert.Contains(typeof(SFA.DAS.Authorization.CommitmentPermissions.AuthorizationHandler), sut.SupportsHandlerTypes);
        }

        [Test]
        public void GetAuthorizationKey_WithValuesInContext_ShouldNotReturnNullKey()
        {
            var fixtures = new AuthorizationContextCacheKeyProviderTestFixtures()
                .WithContext(Party.Employer, 2, 3);

            fixtures.AssertHashKey(Assert.IsNotNull);
        }

        [Test]
        public void GetAuthorizationKey_WithPartyIdInContext_HasPartyTypeSet()
        {
            var fixtures = new AuthorizationContextCacheKeyProviderTestFixtures()
                .WithContext(Party.Employer, 2, 3);

            fixtures.AssertHashKey(key => Assert.AreEqual(Party.Employer, key.PartyType));
        }

        [Test]
        public void GetAuthorizationKey_WithPartyIdInContext_HasPartyIdSet()
        {
            var fixtures = new AuthorizationContextCacheKeyProviderTestFixtures()
                .WithContext(Party.Employer, 2, 3);

            fixtures.AssertHashKey(key => Assert.AreEqual(2, key.PartytId));
        }

        [Test]
        public void GetAuthorizationKey_WithCohortIdInContext_HasCohortIdSet()
        {
            var fixtures = new AuthorizationContextCacheKeyProviderTestFixtures()
                .WithContext(Party.Employer, 2, 3);

            fixtures.AssertHashKey(key => Assert.AreEqual(3, key.CohortId));
        }

        [Test]
        public void GetAuthorizationKey_WithOptionsSupplied_HasOptionsNotNull()
        {
            var fixtures = new AuthorizationContextCacheKeyProviderTestFixtures()
                .WithContext(Party.Employer, 2, 3);

            fixtures.AssertHashKey(key => Assert.IsNotNull(key.Options), new []{"A", "B", "C"});
        }

        [Test]
        public void GetAuthorizationKey_WithOptionsSupplied_HasOptionsWithExpectedValues()
        {
            var fixtures = new AuthorizationContextCacheKeyProviderTestFixtures()
                .WithContext(Party.Employer, 2, 3);

            fixtures.AssertHashKey(key =>
            {
                Assert.IsTrue(key.Options.First() == "A");
                Assert.IsTrue(key.Options.Skip(1).First() == "B");
                Assert.IsTrue(key.Options.Skip(2).First() == "C");
            }, new[] { "A", "B", "C" });
        }

        [Test]
        public void ConfigureCacheItem_WithCacheItem_ShouldSetSlidingExpiration()
        {
            var fixtures = new AuthorizationContextCacheKeyProviderTestFixtures();

            fixtures.AssertSetsExpiration(ce => Assert.IsNotNull(ce.SlidingExpiration));
        }

        [Test]
        public void ConfigureCacheItem_WithCacheItem_ShouldSetSlidingExpirationToSomethingReasonable()
        {
            var fixtures = new AuthorizationContextCacheKeyProviderTestFixtures();

            fixtures.AssertSetsExpiration(ce => Assert.GreaterOrEqual(TimeSpan.FromMinutes(60),  ce.SlidingExpiration));
        }
    }

    public class AuthorizationContextCacheKeyProviderTestFixtures
    {
        public AuthorizationContextCacheKeyProviderTestFixtures()
        {
            AuthorizationContextMock = new Mock<IAuthorizationContext>();
        }

        public Mock<IAuthorizationContext> AuthorizationContextMock { get;  }
        public IAuthorizationContext AuthorizationContext => AuthorizationContextMock.Object;

        public AuthorizationContextCacheKeyProvider CreateSut()
        {
            return new AuthorizationContextCacheKeyProvider();
        }

        public AuthorizationContextCacheKeyProviderTestFixtures WithContext(Party? partyType, long? partyId, long? cohortId)
        {
            SetContextIfNotNull(AuthorizationContextKey.Party, partyType);
            SetContextIfNotNull(AuthorizationContextKey.PartyId, partyId);
            SetContextIfNotNull(AuthorizationContextKey.CohortId, cohortId);
            return this;
        }

        public AuthorizationContextCacheKeyProviderTestFixtures AssertHashKey(Action<CommitmentAuthorizationHashKey> checker)
        {
            return AssertHashKey(checker, null);
        }

        public AuthorizationContextCacheKeyProviderTestFixtures AssertHashKey(Action<CommitmentAuthorizationHashKey> checker, IReadOnlyCollection<string> options)
        {
            var sut = CreateSut();
            var hashkey = sut.GetAuthorizationKey(options, AuthorizationContext);

            checker(hashkey as CommitmentAuthorizationHashKey);

            return this;
        }

        public AuthorizationContextCacheKeyProviderTestFixtures AssertSetsExpiration(Action<ICacheEntry> checker)
        {
            var sut = CreateSut();
            var key = new CommitmentAuthorizationHashKey(Party.Employer, 2, 3, new []{"X","Y", "Z"});

            var cacheEntryMock = new Mock<ICacheEntry>();

            cacheEntryMock.SetupAllProperties();

            sut.ConfigureCacheItem(cacheEntryMock.Object, AuthorizationContext, key, new AuthorizationResult());

            checker(cacheEntryMock.Object);

            return this;
        }

        private void SetContextIfNotNull<T>(string name, T value)
        {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));

            if (underlyingType == null)
            {
                return;
            }

            AuthorizationContextMock
                .Setup(ac => ac.TryGet(name, out value))
                .Returns(true);
        }
    }
}
