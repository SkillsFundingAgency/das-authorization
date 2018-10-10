#if NET462
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    [TestFixture]
    public class AuthorizationFilterTests : FluentTest<AuthorizationFilterTestsFixture>
    {
        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionIsDecoratedWithADasAuthorizeAttributeAndTheOperationIsAuthorized_ThenShouldNotSetResult()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetNoControllerDasAuthorizeAttribute().SetIsAuthorized(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndControllerIsDecoratedWithADasAuthorizeAttributeAndTheOperationIsAuthorized_ThenShouldNotSetResult()
        {
            Run(f => f.SetNoActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute().SetIsAuthorized(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }
        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionAndControllerAreDecoratedWithADasAuthorizeAttributeAndTheOperationIsAuthorized_ThenShouldNotSetResult()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute().SetIsAuthorized(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionIsDecoratedWithADasAuthorizeAttributeAndTheOperationIsNotAuthorized_ThenShouldSetResult()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetNoControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndControllerIsDecoratedWithADasAuthorizeAttributeAndTheOperationIsNotAuthorized_ThenShouldSetResult()
        {
            Run(f => f.SetNoActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionAndControllerAreDecoratedWithADasAuthorizeAttributeAndTheOperationIsNotAuthorized_ThenShouldSetResult()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionAndControllerAreNotDecoratedWithADasAuthorizeAttribute_ThenShouldNotSetResult()
        {
            Run(f => f.SetNoActionDasAuthorizeAttribute().SetNoControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingMoreThanOnce_ThenShouldNotGetDasAuthorizeAttributeMoreThanOnce()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecutingMoreThanOnce(), f =>
            {
                f.ActionDescriptor.Verify(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true), Times.Once);
                f.ActionDescriptor.Verify(d => d.ControllerDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), true), Times.Once);
            });
        }
    }

    public class AuthorizationFilterTestsFixture
    {
        public ActionExecutingContext ActionExecutingContext { get; set; }
        public AuthorizationFilter AuthorizationFilter { get; set; }
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
        public Mock<ActionDescriptor> ActionDescriptor { get; set; }
        public string[] ActionOptions { get; set; }
        public string[] ControllerOptions { get; set; }

        public AuthorizationFilterTestsFixture()
        {
            ActionDescriptor = new Mock<ActionDescriptor>();
            ActionExecutingContext = new ActionExecutingContext { ActionDescriptor = ActionDescriptor.Object };
            AuthorizationService = new Mock<IAuthorizationService>();

            ActionDescriptor.Setup(d => d.UniqueId).Returns(Guid.NewGuid().ToString);

            AuthorizationFilter = new AuthorizationFilter(() => AuthorizationService.Object);
        }

        public void OnActionExecuting()
        {
            AuthorizationFilter.OnActionExecuting(ActionExecutingContext);
        }

        public void OnActionExecutingMoreThanOnce()
        {
            AuthorizationFilter.OnActionExecuting(ActionExecutingContext);
            AuthorizationFilter.OnActionExecuting(ActionExecutingContext);
        }

        public AuthorizationFilterTestsFixture SetActionDasAuthorizeAttribute()
        {
            ActionOptions = new[] { "Action.Option" };
            ActionDescriptor.Setup(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] { new DasAuthorizeAttribute(ActionOptions) });

            return this;
        }

        public AuthorizationFilterTestsFixture SetControllerDasAuthorizeAttribute()
        {
            ControllerOptions = new[] { "Controller.Option" };
            ActionDescriptor.Setup(d => d.ControllerDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] { new DasAuthorizeAttribute(ControllerOptions) });

            return this;
        }

        public AuthorizationFilterTestsFixture SetIsAuthorized()
        {
            AuthorizationService.Setup(a => a.IsAuthorized(It.Is<string[]>(o => ActionOptions.Concat(ControllerOptions).All(o.Contains)))).Returns(true);

            return this;
        }

        public AuthorizationFilterTestsFixture SetNoActionDasAuthorizeAttribute()
        {
            ActionOptions = new string[0];
            ActionDescriptor.Setup(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] {});

            return this;
        }

        public AuthorizationFilterTestsFixture SetNoControllerDasAuthorizeAttribute()
        {
            ControllerOptions = new string[0];
            ActionDescriptor.Setup(d => d.ControllerDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] { });

            return this;
        }
    }
}
#endif