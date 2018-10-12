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
        public void OnActionExecuting_WhenActionIsExecutingAndActionIsDecoratedWithDasAuthorizeAttributeAndActionOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetAuthorizedActionOptions(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndControllerIsDecoratedWithDasAuthorizeAttributeAndControllerOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            Run(f => f.SetControllerDasAuthorizeAttribute().SetAuthorizedControllerOptions(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreDecoratedWithDasAuthorizeAttributeAndActionAndControllerOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute().SetAuthorizedActionOptions().SetAuthorizedControllerOptions(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionIsDecoratedWithDasAuthorizeAttributeAndActionOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            Run(f => f.SetActionDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndControllerIsDecoratedWithDasAuthorizeAttributeAndControllerOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            Run(f => f.SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreDecoratedWithDasAuthorizeAttributeAndActionAndControllerOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreNotDecoratedWithDasAuthorizeAttribute_ThenShouldNotSetResult()
        {
            Run(f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingMoreThanOnce_ThenShouldNotGetDasAuthorizeAttributeMoreThanOnce()
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
            ActionOptions = new string[0];
            ControllerOptions = new string[0];

            ActionDescriptor.Setup(d => d.ControllerDescriptor.ControllerName).Returns(Guid.NewGuid().ToString());
            ActionDescriptor.Setup(d => d.ControllerDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] {});
            ActionDescriptor.Setup(d => d.ActionName).Returns(Guid.NewGuid().ToString());
            ActionDescriptor.Setup(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] {});
            
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

        public AuthorizationFilterTestsFixture SetAuthorizedActionOptions()
        {
            AuthorizationService.Setup(a => a.IsAuthorized(It.Is<string[]>(o => ActionOptions.All(o.Contains)))).Returns(true);

            return this;
        }

        public AuthorizationFilterTestsFixture SetAuthorizedControllerOptions()
        {
            AuthorizationService.Setup(a => a.IsAuthorized(It.Is<string[]>(o => ControllerOptions.All(o.Contains)))).Returns(true);

            return this;
        }
    }
}
#endif