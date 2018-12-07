#if NET462
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