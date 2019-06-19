using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Cache;
using SFA.DAS.Authorization.CommitmentPermissions.Cache;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests.Cache
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationResultCacheConfigurationProviderTests : FluentTest<AuthorizationResultCacheConfigurationProviderTestsFixture>
    {
        [Test]
        public void GetCacheKey_WhenGettingKey_ThenShouldReturnKey()
        {
            Test(f => f.GetCacheKey(), (f, r) => r.Should().NotBeNull().And.Match<CacheKey>(k =>
                k.Options == f.Options &&
                k.CohortId == f.CohortId &&
                k.Party == f.Party &&
                k.PartyId == f.PartyId));
        }

        [Test]
        public void ConfigureCacheEntry_WhenConfiguringCacheEntry_ThenShouldConfigureCacheEntry()
        {
            Test(f => f.ConfigureCacheEntry(), f => f.CacheEntry.Object.Should().Match<ICacheEntry>(e => 
                e.SlidingExpiration == f.SlidingExpiration));
        }
    }

    public class AuthorizationResultCacheConfigurationProviderTestsFixture
    {
        public IReadOnlyCollection<string> Options { get; set; }
        public long CohortId { get; set; }
        public Party Party { get; set; }
        public long PartyId { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public Mock<ICacheEntry> CacheEntry { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public IAuthorizationResultCacheConfigurationProvider AuthorizationResultCacheConfigurationProvider { get; set; }
        
        public AuthorizationResultCacheConfigurationProviderTestsFixture()
        {
            Options = new List<string> { "A", "B", "C" };
            CohortId = 1;
            Party = Party.Employer;
            PartyId = 2;
            AuthorizationContext = new AuthorizationContext();
            SlidingExpiration = TimeSpan.FromMinutes(60);
            CacheEntry = new Mock<ICacheEntry>();
            
            AuthorizationContext.AddCommitmentPermissionValues(CohortId, Party, PartyId);
            CacheEntry.SetupAllProperties();
            
            AuthorizationResultCacheConfigurationProvider = new AuthorizationResultCacheConfigurationProvider();
        }
        
        public object GetCacheKey()
        {
            return AuthorizationResultCacheConfigurationProvider.GetCacheKey(Options, AuthorizationContext);
        }

        public void ConfigureCacheEntry()
        {
            AuthorizationResultCacheConfigurationProvider.ConfigureCacheEntry(CacheEntry.Object);
        }
    }
}
