using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Errors;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;
using SFA.DAS.Authorization.Services;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.UnitTests.Services
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationServiceWithDefaultHandlerTests : FluentTest<AuthorizationServiceWithDefaultHandlerTestsFixture>
    {
        [Test]
        public void Authorize_WhenOperationIsAuthorized_ThenShouldNotThrowException()
        {
            Test(f => f.SetAuthorizedOptions(), f => f.Authorize(), f => { });
        }

        [Test]
        public Task IsAuthorizedAsync_WhenOperationIsAuthorized_ThenShouldReturnTrue()
        {
            return TestAsync(f => f.SetAuthorizedOptions(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public Task GetAuthorizationResultAsync_WhenOperationIsAuthorized_ThenShouldReturnValidAuthorizationResult()
        {
            return TestAsync(f => f.SetAuthorizedOptions(), f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
        }

        [Test]
        public void IsAuthorized_WhenOperationIsAuthorized_ThenShouldReturnTrue()
        {
            Test(f => f.SetAuthorizedOptions(), f => f.IsAuthorized(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public Task AuthorizeAsync_WhenOperationIsAuthorized_ThenShouldNotThrowException()
        {
            return TestAsync(f => f.SetAuthorizedOptions(), f => f.AuthorizeAsync(), f => { });
        }

        [Test]
        public Task IsAuthorizedAsync_WhenOperationIsUnauthorized_ThenShouldReturnFalse()
        {
            return TestAsync(f => f.SetUnauthorizedOptions(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public Task IsAuthorizedAsync_WhenOperationIsUnauthorized_ThenShouldCombineErrorList()
        {            
            return TestAsync(f => f.SetUnauthorizedOptions(),
                f => f.GetAuthorizationResultAsync(),
                (f, r) => r.Should().NotBeNull()
              .And.Match<AuthorizationResult>(r2 => r2.IsAuthorized == false && r2.Errors.Count() == 2
              && r2.HasError<Tier2UserAccesNotGranted>()
              && r2.HasError<EmployerUserRoleNotAuthorized>()));
        }

        [Test]
        public Task IsAuthorizedAsync_WhenDefaultHandlerIsUnauthorized_ThenShouldReturnFalse()
        {
            return TestAsync(f => f.SetUnauthorizedOptionsforDefaultHandler(), f => f.IsAuthorizedAsync(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public void Verify_IsAuthorisedAsync_Called()
        {
            //Arrange

            Test(f => f.Test());
        }


        [Test]
        public void GetAuthorizationResult_WhenOperationisAuthorized_ThenVerifyMethodCalledOnce()
        {
            Test(f => f.SetAuthorizationResult(), f => f.GetAuthorizationResult(), f => f.VerifyAuthorizationService());
        }

        [Test]
        public void GetAuthorizationResultAsync_WhenOperationisAuthorized_ThenVerifyMethodCalledOnce()
        {
            Test(f => f.SetAuthorizationResultAsync(), f => f.GetAuthorizationResultAsync(), f => f.VerifyAuthorizationResultAsyncService());
        }


        [Test]
        public void AuthorizeAsync_WhenOperationisAuthorized_ThenVerifyMethodCalledOnce()
        {
            Test(f => f.SetAuthorizeAsync(), f => f.AuthorizeAsync(), f => f.VerifyAuthorizeAsyncService());
        }

        [Test]
        public void Authorize_WhenOperationisAuthorized_ThenVerifyMethodCalledOnce()
        {
            Test(f => f.Authorize(), f => f.VerifyAuthorize());
        }
    }

        public class AuthorizationServiceWithDefaultHandlerTestsFixture
    {
        public string[] Options { get; set; }
        public Mock<IAuthorizationContextProvider> AuthorizationContextProvider { get; set; }
        public Mock<IAuthorizationContext> AuthorizationContext { get; set; }       
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
        public IAuthorizationService AuthorizationServiceWithDefaultHandler { get; set; }
        public Mock<IDefaultAuthorizationHandler> DefaultAuthorizationHandler { get; set; }      
        
        public Mock<AuthorizationServiceWithDefaultHandler> AuthorizationServiceWithDefaultHandlerMock { get;  set; }



        public AuthorizationServiceWithDefaultHandlerTestsFixture()
        {
            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContext = new Mock<IAuthorizationContext>();
            DefaultAuthorizationHandler = new Mock<IDefaultAuthorizationHandler>();
            AuthorizationService = new Mock<IAuthorizationService>();
            AuthorizationServiceWithDefaultHandlerMock = new Mock<AuthorizationServiceWithDefaultHandler>();

            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(AuthorizationContext.Object);            
            AuthorizationService.Setup(a => a.AuthorizeAsync(Options)).Returns(Task.FromResult(true));            
            AuthorizationService.Setup(a => a.GetAuthorizationResult(Options)).Returns(new AuthorizationResult());
            AuthorizationService.Setup(a => a.GetAuthorizationResultAsync()).ReturnsAsync(new AuthorizationResult());
            AuthorizationService.Setup(a => a.IsAuthorized(Options)).Returns(true);
            AuthorizationService.Setup(a => a.IsAuthorizedAsync(Options)).Returns(Task.FromResult(true));                        

            //TO DO : change name to sut
            AuthorizationServiceWithDefaultHandler = new AuthorizationServiceWithDefaultHandler(
                                                        AuthorizationContextProvider.Object,
                                                        DefaultAuthorizationHandler.Object, 
                                                        AuthorizationService.Object);            
          
        }

        public void Test()
        {
            AuthorizationService.Setup(x => x.GetAuthorizationResult(Options)).Returns(new AuthorizationResult());

            AuthorizationServiceWithDefaultHandler.GetAuthorizationResult(Options);

            AuthorizationService.Verify(x => x.GetAuthorizationResult(Options), Times.Once);            
        }
       


        public void Authorize()
        {
            AuthorizationServiceWithDefaultHandler.Authorize(Options);
        }

        public Task AuthorizeAsync()
        {
            return AuthorizationServiceWithDefaultHandler.AuthorizeAsync(Options);
        }

        public bool IsAuthorized()
        {
            return AuthorizationServiceWithDefaultHandler.IsAuthorized(Options);
        }

        public AuthorizationResult GetAuthorizationResult()
        {
            return AuthorizationServiceWithDefaultHandler.GetAuthorizationResult(Options);
        }
      
        public Task<AuthorizationResult> GetAuthorizationResultAsync()
        {
            return AuthorizationServiceWithDefaultHandler.GetAuthorizationResultAsync(Options);
        }

        public Task<AuthorizationResult> GetDefaultAuthorizationResultAsync()
        {
            var defaultAuthorizationResult = AuthorizationServiceWithDefaultHandler.GetAuthorizationResultAsync(Options);

            return defaultAuthorizationResult;
        }

        public Task<bool> IsAuthorizedAsync()
        {
            return AuthorizationServiceWithDefaultHandler.IsAuthorizedAsync(Options);
        }

        public AuthorizationServiceWithDefaultHandlerTestsFixture SetAuthorizedOptions()
        {
            DefaultAuthorizationHandler.Setup(d => d.GetAuthorizationResult(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult());

            DefaultAuthorizationHandler.Setup(d => d.GetAuthorizationResult(new[] { "Test"} , AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult());
            
            return this;
        }

        public AuthorizationServiceWithDefaultHandlerTestsFixture SetUnauthorizedOptions()
        {
            DefaultAuthorizationHandler.Setup(d => d.GetAuthorizationResult(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult().AddError(new Tier2UserAccesNotGranted()));

            AuthorizationService.Setup(a => a.GetAuthorizationResultAsync()).ReturnsAsync(new AuthorizationResult().AddError(new EmployerUserRoleNotAuthorized()));

            DefaultAuthorizationHandler.Setup(d => d.GetAuthorizationResult(new[] { "TestUnAuthorised" }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult().AddError(new Tier2UserAccesNotGranted()));

            return this;
        }


        public AuthorizationServiceWithDefaultHandlerTestsFixture SetUnauthorizedOptionsforDefaultHandler()
        {
            DefaultAuthorizationHandler.Setup(d => d.GetAuthorizationResult(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult().AddError(new Tier2UserAccesNotGranted()));
                        
            DefaultAuthorizationHandler.Setup(d => d.GetAuthorizationResult(new[] { "TestUnAuthorised" }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult().AddError(new Tier2UserAccesNotGranted()));

            return this;
        }

        public AuthorizationServiceWithDefaultHandlerTestsFixture SetAuthorizationResult()
        {
            AuthorizationService.Setup(x => x.GetAuthorizationResult(Options)).Returns(new AuthorizationResult());

            return this;
        }        

        public AuthorizationServiceWithDefaultHandlerTestsFixture SetAuthorizationResultAsync()
        {
            AuthorizationService.Setup(x => x.GetAuthorizationResultAsync()).ReturnsAsync(new AuthorizationResult());

            DefaultAuthorizationHandler.Setup(x => x.GetAuthorizationResult(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult());

            return this;
        }

       

        public AuthorizationServiceWithDefaultHandlerTestsFixture VerifyAuthorizationService()
        {
            AuthorizationService.Verify(x => x.GetAuthorizationResult(Options), Times.Once);

            return this;
        }


        public AuthorizationServiceWithDefaultHandlerTestsFixture VerifyAuthorizationResultAsyncService()
        {
            AuthorizationService.Verify(x => x.GetAuthorizationResultAsync(), Times.Once);

            DefaultAuthorizationHandler.Verify(x => x.GetAuthorizationResult(Options, AuthorizationContext.Object), Times.Once);

            return this;
        }


        public AuthorizationServiceWithDefaultHandlerTestsFixture SetAuthorizeAsync()
        {
            AuthorizationService.Setup(x => x.AuthorizeAsync(Options)).Returns(Task.FromResult(true));

            return this;
        }


        public AuthorizationServiceWithDefaultHandlerTestsFixture VerifyAuthorizeAsyncService()
        {
            AuthorizationService.Verify(x => x.AuthorizeAsync(Options), Times.Once);

            return this;
        }

        
        public AuthorizationServiceWithDefaultHandlerTestsFixture VerifyAuthorize()
        {
            AuthorizationService.Verify(x => x.Authorize(Options), Times.Once);
            
            return this;
        }


    }
    public class Tier2UserAccesNotGranted : AuthorizationError
    {
        public Tier2UserAccesNotGranted() : base("Tier2 User permission is not granted")
        {

        }
    }

    public class EmployerUserRoleNotAuthorized : AuthorizationError
    {
        public EmployerUserRoleNotAuthorized() : base("Employer user role is not authorized")
        {
        }
    }

}
