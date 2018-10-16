#if NET462
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    [TestFixture]
    public class HtmlHelperExtensionsTests : FluentTest<HtmlHelperExtensionsTestsFixture>
    {
        [Test]
        public void IsAuthorized_WhenOperationIsAuthorized_ThenShouldReturnTrue()
        {
            Run(f => f.SetAuthorized(), f => f.IsAuthorized(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public void IsAuthorized_WhenOperationIsUnauthorized_ThenShouldReturnFalse()
        {
            Run(f => f.SetUnauthorized(), f => f.IsAuthorized(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public void GetAuthorizationResult_WhenGettingAuthorizationResult_ThenShouldReturnAuthorizationResult()
        {
            Run(f => f.SetAuthorizationResult(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.BeSameAs(f.AuthorizationResult));
        }
    }

    public class HtmlHelperExtensionsTestsFixture
    {
        public string[] Options { get; set; }
        public HtmlHelper HtmlHelper { get; set; }
        public Mock<IDependencyResolver> Resolver { get; set; }
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
        public AuthorizationResult AuthorizationResult { get; set; }

        public HtmlHelperExtensionsTestsFixture()
        {
            Options = new [] { "Options.Foo", "Options.Bar" };
            Resolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(Resolver.Object);
            AuthorizationService = new Mock<IAuthorizationService>();

            Resolver.Setup(r => r.GetService(typeof(IAuthorizationService))).Returns(AuthorizationService.Object);

            HtmlHelper = new HtmlHelper(new ViewContext(), Mock.Of<IViewDataContainer>());
        }

        public bool IsAuthorized()
        {
            return HtmlHelper.IsAuthorized(Options);
        }

        public AuthorizationResult GetAuthorizationResult()
        {
            return HtmlHelper.GetAuthorizationResult(Options);
        }

        public HtmlHelperExtensionsTestsFixture SetAuthorized()
        {
            AuthorizationService.Setup(a => a.IsAuthorized(Options)).Returns(true);

            return this;
        }

        public HtmlHelperExtensionsTestsFixture SetUnauthorized()
        {
            AuthorizationService.Setup(a => a.IsAuthorized()).Returns(false);

            return this;
        }

        public HtmlHelperExtensionsTestsFixture SetAuthorizationResult()
        {
            AuthorizationResult = new AuthorizationResult();
            AuthorizationService.Setup(a => a.GetAuthorizationResult(Options)).Returns(AuthorizationResult);

            return this;
        }
    }
}
#endif