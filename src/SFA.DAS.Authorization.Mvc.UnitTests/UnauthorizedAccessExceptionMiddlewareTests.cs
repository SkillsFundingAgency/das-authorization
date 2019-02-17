#if NETCOREAPP2_0
using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class UnauthorizedAccessExceptionMiddlewareTests : FluentTest<UnauthorizedAccessExceptionMiddlewareTestsFixture>
    {
        [Test]
        public Task OnException_WhenAnUnauthorizedAccessExceptionIsThrown_ThenShouldSetForbiddenResponseStatusCode()
        {
            return TestAsync(f => f.SetUnauthorizedAccessException(), f => f.InvokeAsync(), f => f.Context.Response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden));
        }

        [Test]
        public Task OnException_WhenAnUnauthorizedAccessExceptionIsThrown_ThenShouldNotThrowException()
        {
            return TestExceptionAsync(f => f.SetUnauthorizedAccessException(), f => f.InvokeAsync(), (f, r) => r.Should().NotThrow());
        }

        [Test]
        public Task OnException_WhenAnExceptionIsThrown_ThenShouldNotSetForbiddenResponseStatusCode()
        {
            return TestAsync(f => f.SetException(), f => f.InvokeAsyncAndSwallowException(), f => f.Context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK));
        }

        [Test]
        public Task OnException_WhenAnExceptionIsThrown_ThenShouldThrowException()
        {
            return TestExceptionAsync(f => f.SetException(), f => f.InvokeAsync(), (f, r) => r.Should().Throw<Exception>());
        }
    }

    public class UnauthorizedAccessExceptionMiddlewareTestsFixture
    {
        public HttpContext Context { get; set; }
        public Mock<RequestDelegate> NextTask { get; set; }
        public UnauthorizedAccessExceptionMiddleware UnauthorizedAccessExceptionMiddleware { get; set; }
        public Exception Exception { get; set; }

        public UnauthorizedAccessExceptionMiddlewareTestsFixture()
        {
            Context = new DefaultHttpContext();
            NextTask = new Mock<RequestDelegate>();
            UnauthorizedAccessExceptionMiddleware = new UnauthorizedAccessExceptionMiddleware(NextTask.Object);
        }

        public Task InvokeAsync()
        {
            return UnauthorizedAccessExceptionMiddleware.InvokeAsync(Context);
        }

        public async Task InvokeAsyncAndSwallowException()
        {
            try
            {
                await UnauthorizedAccessExceptionMiddleware.InvokeAsync(Context);
            }
            catch
            {
            }
        }

        public UnauthorizedAccessExceptionMiddlewareTestsFixture SetUnauthorizedAccessException()
        {
            Exception = new UnauthorizedAccessException();

            NextTask.Setup(n => n(Context)).ThrowsAsync(Exception);

            return this;
        }

        public UnauthorizedAccessExceptionMiddlewareTestsFixture SetException()
        {
            Exception = new Exception();

            NextTask.Setup(n => n(Context)).ThrowsAsync(Exception);

            return this;
        }
    }
}
#endif