#if NET462
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.WebApi.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class UnauthorizedAccessExceptionFilterTests : FluentTest<UnauthorizedAccessExceptionFilterTestsFixture>
    {
        [Test]
        public void OnException_WhenAnUnauthorizedAccessExceptionIsThrown_ThenShouldSetResponse()
        {
            Test(f => f.SetUnauthorizedAccessException(), f => f.OnException(), f => f.ActionExecutedContext.Response.Should().NotBeNull().And.Match<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnException_WhenAnExceptionIsThrown_ThenShouldNotSetResponse()
        {
            Test(f => f.SetException(), f => f.OnException(), f => f.ActionExecutedContext.Response.Should().BeNull());
        }
    }

    public class UnauthorizedAccessExceptionFilterTestsFixture
    {
        public HttpActionExecutedContext ActionExecutedContext { get; set; }
        public UnauthorizedAccessExceptionFilter UnauthorizedAccessExceptionFilter { get; set; }
        public HttpRequestMessage HttpRequestMessage { get; set; }
        public HttpControllerContext ControllerContext { get; set; }
        public Mock<HttpActionDescriptor> ActionDescriptor { get; set; }
        public HttpActionContext ActionContext { get; set; }
        public Exception Exception { get; set; }

        public UnauthorizedAccessExceptionFilterTestsFixture()
        {
            HttpRequestMessage = new HttpRequestMessage();
            ControllerContext = new HttpControllerContext { Request = HttpRequestMessage };
            ActionDescriptor = new Mock<HttpActionDescriptor>();
            ActionContext = new HttpActionContext(ControllerContext, ActionDescriptor.Object);
            ActionExecutedContext = new HttpActionExecutedContext(ActionContext, null);
            
            UnauthorizedAccessExceptionFilter = new UnauthorizedAccessExceptionFilter();
        }

        public void OnException()
        {
            UnauthorizedAccessExceptionFilter.OnException(ActionExecutedContext);
        }

        public UnauthorizedAccessExceptionFilterTestsFixture SetUnauthorizedAccessException()
        {
            Exception = new UnauthorizedAccessException();
            ActionExecutedContext.Exception = Exception;

            return this;
        }

        public UnauthorizedAccessExceptionFilterTestsFixture SetException()
        {
            Exception = new Exception();
            ActionExecutedContext.Exception = Exception;

            return this;
        }
    }
}
#endif