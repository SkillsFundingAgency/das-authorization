#if NETCOREAPP2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Authorization.Mvc.Filters;
using SFA.DAS.Authorization.Services;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests.Filters
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationFilterTests : FluentTest<AuthorizationFilterTestsFixture>
    {
        [Test]
        public Task OnActionExecuting_WhenActionIsExecutingAndActionIsDecoratedWithDasAuthorizeAttributeAndActionOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            return TestAsync(f => f.SetActionDasAuthorizeAttribute().SetAuthorizedActionOptions(), f => f.OnAuthorizationAsync(), f => f.AuthorizationFilterContext.Result.Should().BeNull());
        }

        [Test]
        public Task OnActionExecuting_WhenActionIsExecutingAndControllerIsDecoratedWithDasAuthorizeAttributeAndControllerOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            return TestAsync(f => f.SetControllerDasAuthorizeAttribute().SetAuthorizedControllerOptions(), f => f.OnAuthorizationAsync(), f => f.AuthorizationFilterContext.Result.Should().BeNull());
        }

        [Test]
        public Task OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreDecoratedWithDasAuthorizeAttributeAndActionAndControllerOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            return TestAsync(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute().SetAuthorizedActionOptions().SetAuthorizedControllerOptions(), f => f.OnAuthorizationAsync(), f => f.AuthorizationFilterContext.Result.Should().BeNull());
        }

        [Test]
        public Task OnActionExecuting_WhenActionIsExecutingAndActionIsDecoratedWithDasAuthorizeAttributeAndActionOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            return TestAsync(f => f.SetActionDasAuthorizeAttribute(), f => f.OnAuthorizationAsync(), f => f.AuthorizationFilterContext.Result.Should().NotBeNull().And.Match<StatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public Task OnActionExecuting_WhenActionIsExecutingAndControllerIsDecoratedWithDasAuthorizeAttributeAndControllerOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            return TestAsync(f => f.SetControllerDasAuthorizeAttribute(), f => f.OnAuthorizationAsync(), f => f.AuthorizationFilterContext.Result.Should().NotBeNull().And.Match<StatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public Task OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreDecoratedWithDasAuthorizeAttributeAndActionAndControllerOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            return TestAsync(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnAuthorizationAsync(), f => f.AuthorizationFilterContext.Result.Should().NotBeNull().And.Match<StatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public Task OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreNotDecoratedWithDasAuthorizeAttribute_ThenShouldNotSetResult()
        {
            return TestAsync(f => f.OnAuthorizationAsync(), f => f.AuthorizationFilterContext.Result.Should().BeNull());
        }

        [Test]
        public Task OnActionExecuting_WhenActionIsExecutingMoreThanOnce_ThenShouldNotGetDasAuthorizeAttributeMoreThanOnce()
        {
            return TestAsync(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnAuthorizationAsyncMoreThanOnce(), f =>
            {
                f.MethodInfo.Verify(i => i.GetCustomAttributes(typeof(DasAuthorizeAttribute), true), Times.Once);
                f.ControllerTypeInfo.Verify(i => i.GetCustomAttributes(typeof(DasAuthorizeAttribute), true), Times.Once);
            });
        }
    }

    public class AuthorizationFilterTestsFixture
    {
        public AuthorizationFilterContext AuthorizationFilterContext { get; set; }
        public ActionContext ActionContext { get; set; }
        public Mock<HttpContext> HttpContext { get; set; }
        public ControllerActionDescriptor ActionDescriptor { get; set; }
        public Mock<TypeInfo> ControllerTypeInfo { get; set; }
        public Mock<MethodInfo> MethodInfo { get; set; }
        public AuthorizationFilter AuthorizationFilter { get; set; }
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
        public string[] ActionOptions { get; set; }
        public string[] ControllerOptions { get; set; }

        public AuthorizationFilterTestsFixture()
        {
            ControllerTypeInfo = new Mock<TypeInfo>();
            MethodInfo = new Mock<MethodInfo>();
            
            ActionDescriptor = new ControllerActionDescriptor
            {
                ControllerName = Guid.NewGuid().ToString(),
                ControllerTypeInfo = ControllerTypeInfo.Object,
                ActionName = Guid.NewGuid().ToString(),
                MethodInfo = MethodInfo.Object
            };
            
            HttpContext = new Mock<HttpContext>();
            ActionContext = new ActionContext(HttpContext.Object, new RouteData(),  ActionDescriptor);
            AuthorizationFilterContext = new AuthorizationFilterContext(ActionContext, new List<IFilterMetadata>());
            AuthorizationService = new Mock<IAuthorizationService>();
            ActionOptions = new string[0];
            ControllerOptions = new string[0];

            ControllerTypeInfo.Setup(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] {});
            MethodInfo.Setup(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] {});
            
            AuthorizationFilter = new AuthorizationFilter(AuthorizationService.Object);
        }

        public Task OnAuthorizationAsync()
        {
            return AuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext);
        }

        public async Task OnAuthorizationAsyncMoreThanOnce()
        {
            await AuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext);
            await AuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext);
        }

        public AuthorizationFilterTestsFixture SetActionDasAuthorizeAttribute()
        {
            ActionOptions = new[] { "Action.Option" };
            MethodInfo.Setup(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] { new DasAuthorizeAttribute(ActionOptions) });

            return this;
        }

        public AuthorizationFilterTestsFixture SetControllerDasAuthorizeAttribute()
        {
            ControllerOptions = new[] { "Controller.Option" };
            ControllerTypeInfo.Setup(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true)).Returns(new object[] { new DasAuthorizeAttribute(ControllerOptions) });

            return this;
        }

        public AuthorizationFilterTestsFixture SetAuthorizedActionOptions()
        {
            AuthorizationService.Setup(a => a.IsAuthorizedAsync(It.Is<string[]>(o => ActionOptions.All(o.Contains)))).ReturnsAsync(true);

            return this;
        }

        public AuthorizationFilterTestsFixture SetAuthorizedControllerOptions()
        {
            AuthorizationService.Setup(a => a.IsAuthorizedAsync(It.Is<string[]>(o => ControllerOptions.All(o.Contains)))).ReturnsAsync(true);

            return this;
        }
    }
}
#elif NET462
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Authorization.Mvc.Filters;
using SFA.DAS.Authorization.Services;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests.Filters
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationFilterTests : FluentTest<AuthorizationFilterTestsFixture>
    {
        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionIsDecoratedWithDasAuthorizeAttributeAndActionOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            Test(f => f.SetActionDasAuthorizeAttribute().SetAuthorizedActionOptions(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndControllerIsDecoratedWithDasAuthorizeAttributeAndControllerOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            Test(f => f.SetControllerDasAuthorizeAttribute().SetAuthorizedControllerOptions(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreDecoratedWithDasAuthorizeAttributeAndActionAndControllerOptionsAreAuthorized_ThenShouldNotSetResult()
        {
            Test(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute().SetAuthorizedActionOptions().SetAuthorizedControllerOptions(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionIsDecoratedWithDasAuthorizeAttributeAndActionOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            Test(f => f.SetActionDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndControllerIsDecoratedWithDasAuthorizeAttributeAndControllerOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            Test(f => f.SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreDecoratedWithDasAuthorizeAttributeAndActionAndControllerOptionsAreNotAuthorized_ThenShouldSetResult()
        {
            Test(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingAndActionAndControllerAreNotDecoratedWithDasAuthorizeAttribute_ThenShouldNotSetResult()
        {
            Test(f => f.OnActionExecuting(), f => f.ActionExecutingContext.Result.Should().BeNull());
        }

        [Test]
        public void OnActionExecuting_WhenActionIsExecutingMoreThanOnce_ThenShouldNotGetDasAuthorizeAttributeMoreThanOnce()
        {
            Test(f => f.SetActionDasAuthorizeAttribute().SetControllerDasAuthorizeAttribute(), f => f.OnActionExecutingMoreThanOnce(), f =>
            {
                f.ActionDescriptor.Verify(d => d.GetCustomAttributes(typeof(DasAuthorizeAttribute), true), Times.Once);
                f.ActionDescriptor.Verify(d => d.ControllerDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), true), Times.Once);
            });
        }
    }

    public class AuthorizationFilterTestsFixture
    {
        public ActionExecutingContext ActionExecutingContext { get; set; }
        public Mock<ActionDescriptor> ActionDescriptor { get; set; }
        public AuthorizationFilter AuthorizationFilter { get; set; }
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
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