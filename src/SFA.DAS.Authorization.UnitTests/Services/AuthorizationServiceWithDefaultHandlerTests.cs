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
            return TestAsync(f => f.SetAuthorizedOptions(), f => f.GetAuthorizationResultAsync(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationResult>(r2 => r2.IsAuthorized));
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


    }

    public class AuthorizationServiceWithDefaultHandlerTestsFixture
    {
        public string[] Options { get; set; }
        public Mock<IAuthorizationContextProvider> AuthorizationContextProvider { get; set; }
        public Mock<IAuthorizationContext> AuthorizationContext { get; set; }       
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
        public IAuthorizationService AuthorizationServiceWithDefaultHandler { get; set; }
        public Mock<IDefaultAuthorizationHandler> DefaultAuthorizationHandler { get; set; }       
        

        public AuthorizationServiceWithDefaultHandlerTestsFixture()
        {
            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContext = new Mock<IAuthorizationContext>();
            DefaultAuthorizationHandler = new Mock<IDefaultAuthorizationHandler>();
            AuthorizationService = new Mock<IAuthorizationService>();

            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(AuthorizationContext.Object);            
            AuthorizationService.Setup(a => a.AuthorizeAsync(Options)).Returns(Task.FromResult(true));            
            AuthorizationService.Setup(a => a.GetAuthorizationResult(Options)).Returns(new AuthorizationResult());
            AuthorizationService.Setup(a => a.GetAuthorizationResultAsync()).ReturnsAsync(new AuthorizationResult());
            AuthorizationService.Setup(a => a.IsAuthorized(Options)).Returns(true);
            AuthorizationService.Setup(a => a.IsAuthorizedAsync(Options)).Returns(Task.FromResult(true));            
            DefaultAuthorizationHandler.Setup(d => d.GetDefaultAuthorizationResult(Options, AuthorizationContext.Object)).ReturnsAsync(new AuthorizationResult());

            AuthorizationServiceWithDefaultHandler = new AuthorizationServiceWithDefaultHandler(
                                                        AuthorizationContextProvider.Object,
                                                        DefaultAuthorizationHandler.Object, 
                                                        AuthorizationService.Object);            
          
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
            DefaultAuthorizationHandler.Setup(d => d.GetDefaultAuthorizationResult(new[] { "Test"} , AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult());
            
            return this;
        }

        public AuthorizationServiceWithDefaultHandlerTestsFixture SetUnauthorizedOptions()
        {
            DefaultAuthorizationHandler.Setup(d => d.GetDefaultAuthorizationResult(new[] { "TestUnAuthorised" }, AuthorizationContext.Object))
                .ReturnsAsync(new AuthorizationResult().AddError(new Tier2UserAccesNotGranted()));
            
            return this;
        }

    }

    public class Tier2UserAccesNotGranted : AuthorizationError
    {
        public Tier2UserAccesNotGranted() : base("Tier2 User permission is not granted")
        {

        }
    }
}
