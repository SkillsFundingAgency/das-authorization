#if NET462
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Hosting;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.WebApi.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationFilterTests : FluentTest<AuthorizationFilterTestsFixture>
    {
        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionIsDecoratedWithDasAuthorizeAttributeAndActionOptionsAreAuthorized_ThenShouldNotSetResponse()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetAuthorizedActionOptions(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndControllerIsDecoratedWithDasAuthorizeAttributeAndControllerOptionsAreAuthorized_ThenShouldNotSetResponse()
        {
            Run(f => f.SetControllerDasAuthorizeAttribute().SetAuthorizedControllerOptions(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreDecoratedWithDasAuthorizeAttributeAndActionAndControllerOptionsAreAuthorized_ThenShouldNotSetResponse()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute().SetAuthorizedControllerOptions(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionIsDecoratedWithDasAuthorizeAttributeAndActionOptionsAreNotAuthorized_ThenShouldSetResponse()
        {
            Run(f => f.SetActionDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().NotBeNull().And.Match<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndControllerIsDecoratedWithDasAuthorizeAttributeAndControllerOptionsAreNotAuthorized_ThenShouldSetResponse()
        {
            Run(f => f.SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().NotBeNull().And.Match<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreDecoratedWithDasAuthorizeAttributeAndTheOperationsAreNotAuthorized_ThenShouldSetResponse()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().NotBeNull().And.Match<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreNotDecoratedWithDasAuthorizeAttribute_ThenShouldNotSetResponse()
        {
            Run(f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingMoreThanOnce_ThenShouldNotGetTheDasAuthorizeAttributeMoreThanOnce()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecutingMoreThanOnce(), f =>
            {
                f.ActionDescriptor.Verify(d => d.GetCustomAttributes<DasAuthorizeAttribute>(true), Times.Once);
                f.ControllerDescriptor.Verify(d => d.GetCustomAttributes<DasAuthorizeAttribute>(true), Times.Once);
            });
        }
    }

    public class AuthorizationFilterTestsFixture
    {
        public HttpActionContext ActionContext { get; set; }
        public AuthorizationFilter AuthorizationFilter { get; set; }
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
        public HttpRequestMessage HttpRequestMessage { get; set; }
        public HttpControllerContext ControllerContext { get; set; }
        public Mock<HttpActionDescriptor> ActionDescriptor { get; set; }
        public Mock<HttpControllerDescriptor> ControllerDescriptor { get; set; }
        public Mock<IDependencyScope> DependencyScope { get; set; }
        public string[] ActionOptions { get; set; }
        public string[] ControllerOptions { get; set; }

        public AuthorizationFilterTestsFixture()
        {
            HttpRequestMessage = new HttpRequestMessage();
            ControllerContext = new HttpControllerContext { Request = HttpRequestMessage };
            ActionDescriptor = new Mock<HttpActionDescriptor>();
            ControllerDescriptor = new Mock<HttpControllerDescriptor>();
            DependencyScope = new Mock<IDependencyScope>();
            ActionContext = new HttpActionContext(ControllerContext, ActionDescriptor.Object);
            AuthorizationService = new Mock<IAuthorizationService>();
            ActionOptions = new string[0];
            ControllerOptions = new string[0];
            
            ControllerDescriptor.Object.ControllerName = Guid.NewGuid().ToString();
            ControllerDescriptor.Setup(d => d.GetCustomAttributes<DasAuthorizeAttribute>(true)).Returns(new Collection<DasAuthorizeAttribute>());
            ActionDescriptor.Setup(d => d.ActionName).Returns(Guid.NewGuid().ToString());
            ActionDescriptor.Object.ControllerDescriptor = ControllerDescriptor.Object;
            ActionDescriptor.Setup(d => d.GetCustomAttributes<DasAuthorizeAttribute>(true)).Returns(new Collection<DasAuthorizeAttribute>());
            DependencyScope.Setup(s => s.GetService(typeof(IAuthorizationService))).Returns(AuthorizationService.Object);
            HttpRequestMessage.Properties[HttpPropertyKeys.DependencyScope] = DependencyScope.Object;

            AuthorizationFilter = new AuthorizationFilter();
        }

        public void OnActionExecuting()
        {
            AuthorizationFilter.OnActionExecuting(ActionContext);
        }

        public void OnActionExecutingMoreThanOnce()
        {
            AuthorizationFilter.OnActionExecuting(ActionContext);
            AuthorizationFilter.OnActionExecuting(ActionContext);
        }

        public AuthorizationFilterTestsFixture SetActionDasAuthorizeAttribute()
        {
            ActionOptions = new[] { "Action.Option" };
            ActionDescriptor.Setup(d => d.GetCustomAttributes<DasAuthorizeAttribute>(true)).Returns(new Collection<DasAuthorizeAttribute> { new DasAuthorizeAttribute(ActionOptions) });

            return this;
        }

        public AuthorizationFilterTestsFixture SetControllerDasAuthorizeAttribute()
        {
            ControllerOptions = new[] { "Controller.Option" };
            ControllerDescriptor.Setup(d => d.GetCustomAttributes<DasAuthorizeAttribute>(true)).Returns(new Collection<DasAuthorizeAttribute> { new DasAuthorizeAttribute(ControllerOptions) });

            return this;
        }

        public AuthorizationFilterTestsFixture SetAuthorizedControllerOptions()
        {
            AuthorizationService.Setup(a => a.IsAuthorized(It.Is<string[]>(o => ControllerOptions.All(o.Contains)))).Returns(true);

            return this;
        }

        public AuthorizationFilterTestsFixture SetAuthorizedActionOptions()
        {
            AuthorizationService.Setup(a => a.IsAuthorized(It.Is<string[]>(o => ActionOptions.All(o.Contains)))).Returns(true);

            return this;
        }
    }
}
#endif