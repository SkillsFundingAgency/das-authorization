#if NETCOREAPP2_0
using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class UnauthorizedAccessExceptionFilterTests : FluentTest<UnauthorizedAccessExceptionFilterTestsFixture>
    {
        [Test]
        public void OnException_WhenAnUnauthorizedAccessExceptionIsThrown_ThenShouldSetResult()
        {
            Test(f => f.SetUnauthorizedAccessException(), f => f.OnException(), f => f.ExceptionContext.Result.Should().NotBeNull().And.Match<StatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnException_WhenAnUnauthorizedAccessExceptionIsThrown_ThenShouldSetExceptionHandled()
        {
            Test(f => f.SetUnauthorizedAccessException(), f => f.OnException(), f => f.ExceptionContext.ExceptionHandled.Should().BeTrue());
        }

        [Test]
        public void OnException_WhenAnExceptionIsThrown_ThenShouldNotSetResult()
        {
            Test(f => f.SetException(), f => f.OnException(), f => f.ExceptionContext.Result.Should().BeNull());
        }

        [Test]
        public void OnException_WhenAnExceptionIsThrown_ThenShouldNotSetExceptionHandled()
        {
            Test(f => f.SetException(), f => f.OnException(), f => f.ExceptionContext.ExceptionHandled.Should().BeFalse());
        }
    }

    public class UnauthorizedAccessExceptionFilterTestsFixture
    {
        public ExceptionContext ExceptionContext { get; set; }
        public ActionContext ActionContext { get; set; }
        public Mock<HttpContext> HttpContext { get; set; }
        public UnauthorizedAccessExceptionFilter UnauthorizedAccessExceptionFilter { get; set; }
        public Exception Exception { get; set; }

        public UnauthorizedAccessExceptionFilterTestsFixture()
        {
            HttpContext = new Mock<HttpContext>();
            ActionContext = new ActionContext(HttpContext.Object, new RouteData(), new ActionDescriptor());
            ExceptionContext = new ExceptionContext(ActionContext, new List<IFilterMetadata>());
            UnauthorizedAccessExceptionFilter = new UnauthorizedAccessExceptionFilter();
        }

        public void OnException()
        {
            UnauthorizedAccessExceptionFilter.OnException(ExceptionContext);
        }

        public UnauthorizedAccessExceptionFilterTestsFixture SetUnauthorizedAccessException()
        {
            Exception = new UnauthorizedAccessException();
            ExceptionContext.Exception = Exception;

            return this;
        }

        public UnauthorizedAccessExceptionFilterTestsFixture SetException()
        {
            Exception = new Exception();
            ExceptionContext.Exception = Exception;

            return this;
        }
    }
}
#elif NET462
using System;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class UnauthorizedAccessExceptionFilterTests : FluentTest<UnauthorizedAccessExceptionFilterTestsFixture>
    {
        [Test]
        public void OnException_WhenAnUnauthorizedAccessExceptionIsThrown_ThenShouldSetResult()
        {
            Test(f => f.SetUnauthorizedAccessException(), f => f.OnException(), f => f.ExceptionContext.Result.Should().NotBeNull().And.Match<HttpStatusCodeResult>(r => r.StatusCode == (int)HttpStatusCode.Forbidden));
        }

        [Test]
        public void OnException_WhenAnUnauthorizedAccessExceptionIsThrown_ThenShouldSetExceptionHandled()
        {
            Test(f => f.SetUnauthorizedAccessException(), f => f.OnException(), f => f.ExceptionContext.ExceptionHandled.Should().BeTrue());
        }

        [Test]
        public void OnException_WhenAnExceptionIsThrown_ThenShouldNotSetResult()
        {
            Test(f => f.SetException(), f => f.OnException(), f => f.ExceptionContext.Result.Should().NotBeNull().And.BeOfType<EmptyResult>());
        }

        [Test]
        public void OnException_WhenAnExceptionIsThrown_ThenShouldNotSetExceptionHandled()
        {
            Test(f => f.SetException(), f => f.OnException(), f => f.ExceptionContext.ExceptionHandled.Should().BeFalse());
        }
    }

    public class UnauthorizedAccessExceptionFilterTestsFixture
    {
        public ExceptionContext ExceptionContext { get; set; }
        public UnauthorizedAccessExceptionFilter UnauthorizedAccessExceptionFilter { get; set; }
        public Exception Exception { get; set; }

        public UnauthorizedAccessExceptionFilterTestsFixture()
        {
            ExceptionContext = new ExceptionContext();
            UnauthorizedAccessExceptionFilter = new UnauthorizedAccessExceptionFilter();
        }

        public void OnException()
        {
            UnauthorizedAccessExceptionFilter.OnException(ExceptionContext);
        }

        public UnauthorizedAccessExceptionFilterTestsFixture SetUnauthorizedAccessException()
        {
            Exception = new UnauthorizedAccessException();
            ExceptionContext.Exception = Exception;

            return this;
        }

        public UnauthorizedAccessExceptionFilterTestsFixture SetException()
        {
            Exception = new Exception();
            ExceptionContext.Exception = Exception;

            return this;
        }
    }
}
#endif