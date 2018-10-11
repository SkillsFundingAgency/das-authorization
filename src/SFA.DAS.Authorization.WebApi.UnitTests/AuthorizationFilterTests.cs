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
    public class AuthorizationFilterTests : FluentTest<AuthorizationFilterTestsFixture>
    {
        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionIsDecoratedWithADasAuthorizeAttributeAndTheOperationIsAuthorized_ThenShouldNotSetResponse()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetNoControllerDasAuthorizeAttribute().SetIsAuthorized(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndControllerIsDecoratedWithADasAuthorizeAttributeAndTheOperationIsAuthorized_ThenShouldNotSetResponse()
        {
            Run(f => f.SetNoActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute().SetIsAuthorized(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionAndControllerAreDecoratedWithADasAuthorizeAttributeAndTheOperationIsAuthorized_ThenShouldNotSetResponse()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute().SetIsAuthorized(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionIsDecoratedWithADasAuthorizeAttributeAndTheOperationIsNotAuthorized_ThenShouldSetResponse()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetNoControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().NotBeNull().And.Match<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndControllerIsDecoratedWithADasAuthorizeAttributeAndTheOperationIsNotAuthorized_ThenShouldSetResponse()
        {
            Run(f => f.SetNoActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().NotBeNull().And.Match<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionAndControllerAreDecoratedWithADasAuthorizeAttributeAndTheOperationIsNotAuthorized_ThenShouldSetResponse()
        {
            Run(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().NotBeNull().And.Match<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingAndActionAndControllerAreNotDecoratedWithADasAuthorizeAttribute_ThenShouldNotSetResponse()
        {
            Run(f => f.SetNoActionDasAuthorizeAttribute().SetNoControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionContext.Response.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenAnActionIsExecutingMoreThanOnce_ThenShouldNotGetDasAuthorizeAttributeMoreThanOnce()
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
            
            ControllerDescriptor.Object.ControllerName = Guid.NewGuid().ToString();
            ActionDescriptor.Setup(d => d.ActionName).Returns(Guid.NewGuid().ToString());
            ActionDescriptor.Object.ControllerDescriptor = ControllerDescriptor.Object;
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

        public AuthorizationFilterTestsFixture SetIsAuthorized()
        {
            AuthorizationService.Setup(a => a.IsAuthorized(It.Is<string[]>(o => ActionOptions.Concat(ControllerOptions).All(o.Contains)))).Returns(true);

            return this;
        }

        public AuthorizationFilterTestsFixture SetNoActionDasAuthorizeAttribute()
        {
            ActionOptions = new string[0];
            ActionDescriptor.Setup(d => d.GetCustomAttributes<DasAuthorizeAttribute>(true)).Returns(new Collection<DasAuthorizeAttribute>());

            return this;
        }

        public AuthorizationFilterTestsFixture SetNoControllerDasAuthorizeAttribute()
        {
            ControllerOptions = new string[0];
            ControllerDescriptor.Setup(d => d.GetCustomAttributes<DasAuthorizeAttribute>(true)).Returns(new Collection<DasAuthorizeAttribute>());

            return this;
        }
    }
}
#endif