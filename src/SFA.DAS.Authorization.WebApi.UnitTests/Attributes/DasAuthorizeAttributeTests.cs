using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Authorization.WebApi.Attributes;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.WebApi.UnitTests.Attributes
{
    [TestFixture]
    [Parallelizable]
    public class DasAuthorizeAttributeTests : FluentTest<DasAuthorizeAttributeTestsFixture>
    {
        [Test]
        public void Constructor_WhenConstructingADasAuthorizeAttributeWithOptions_ThenShouldConstructADasAuthorizeAttribute()
        {
            Test(f => new DasAuthorizeAttribute(f.Options), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Options.Should().BeSameAs(f.Options);
            });
        }

        [Test]
        public void Constructor_WhenConstructingADasAuthorizeAttributeWithNullOptions_ThenShouldThrowAnException()
        {
            TestException(f => new DasAuthorizeAttribute(null), (f, r) => r.Should().Throw<ArgumentNullException>());
        }
    }

    public class DasAuthorizeAttributeTestsFixture
    {
        public string[] Options { get; set; }

        public DasAuthorizeAttributeTestsFixture()
        {
            Options = new [] { "Foo.Bar" };
        }
    }
}