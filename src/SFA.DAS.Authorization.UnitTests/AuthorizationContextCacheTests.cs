﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationContextCacheTests : FluentTest<AuthorizationContextCacheTestsFixture>
    {
        [Test]
        public void GetAuthorizationContext_WhenGettingAuthorizationContext_ThenShouldReturnAuthorizationContext()
        {
            Test(f => f.GetAuthorizationContext(), (f, r) => r.Should().NotBeNull());
        }

        [Test]
        public void GetAuthorizationContext_WhenGettingAuthorizationContext_ThenShouldGetAuthorizationContextFromAuthorizationContextProvider()
        {
            Test(f => f.GetAuthorizationContext(), f => f.AuthorizationContextProvider.Verify(p => p.GetAuthorizationContext(), Times.Once));
        }

        [Test]
        public void GetAuthorizationContext_WhenGettingAuthorizationContextMultipleTimes_ThenShouldGetAuthorizationContextFromAuthorizationContextProviderOnce()
        {
            Test(f => f.GetAuthorizationContext(), f => f.AuthorizationContextProvider.Verify(p => p.GetAuthorizationContext(), Times.Once));
        }

        [Test]
        public void GetAuthorizationContext_WhenGettingAuthorizationContextMultipleTimes_ThenShouldReturnSameAuthorizationContext()
        {
            Test(f => f.GetAuthorizationContext(3), (f, r) => f.AuthorizationContexts.ForEach(c => c.Should().Be(r)));
        }
    }

    public class AuthorizationContextCacheTestsFixture
    {
        public Mock<IAuthorizationContextProvider> AuthorizationContextProvider { get; set; }
        public IAuthorizationContextProvider AuthorizationContextCache { get; set; }
        public List<IAuthorizationContext> AuthorizationContexts { get; set; }

        public AuthorizationContextCacheTestsFixture()
        {
            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContextCache = new AuthorizationContextCache(AuthorizationContextProvider.Object);
            AuthorizationContexts = new List<IAuthorizationContext>();

            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(() => new AuthorizationContext());
        }

        public IAuthorizationContext GetAuthorizationContext(int? count = 1)
        {
            for (var i = 0; i < count; i++)
            {
                AuthorizationContexts.Add(AuthorizationContextCache.GetAuthorizationContext());
            }

            return AuthorizationContexts.First();
        }
    }
}