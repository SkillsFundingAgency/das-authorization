#if NET462
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationModelBinderTests : FluentTest<AuthorizationModelBinderTestsFixture>
    {
        [Test]
        public void BindModel_WhenBindingAnAuthorizationContextModelAndAPropertyNameExistsInTheAuthorizationContext_ThenShouldSetThePropertyValue()
        {
            Test(f => f.SetAuthorizationContext(), f => f.BindModel(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.UserRef.Should().Be(f.UserRef);
            });
        }

        [Test]
        public void BindModel_WhenBindingAnAuthorizationContextModelAndAPropertyNameDoesNotExistInTheAuthorizationContext_ThenShouldNotSetThePropertyValue()
        {
            Test(f => f.BindModel(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.UserRef.Should().BeNull();
            });
        }
    }

    public class AuthorizationModelBinderTestsFixture
    {
        public Guid UserRef { get; set; }
        public ControllerContext ControllerContext { get; set; }
        public ModelBindingContext BindingContext { get; set; }
        public IValueProvider ValueProvider { get; set; }
        public DefaultModelBinder ModelBinder { get; set; }
        public Mock<IAuthorizationContextProvider> AuthorizationContextProvider { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }

        public AuthorizationModelBinderTestsFixture()
        {
            UserRef = Guid.NewGuid();
            ControllerContext = new ControllerContext(Mock.Of<HttpContextBase>(), new RouteData(), Mock.Of<ControllerBase>());
            ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), null);

            BindingContext = new ModelBindingContext
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(AuthorizationContextModelStub)),
                ModelName = "",
                ValueProvider = ValueProvider
            };

            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContext = new AuthorizationContext();

            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(AuthorizationContext);

            ModelBinder = new AuthorizationModelBinder(() => AuthorizationContextProvider.Object);
        }

        public AuthorizationContextModelStub BindModel()
        {
            return ModelBinder.BindModel(ControllerContext, BindingContext) as AuthorizationContextModelStub;
        }

        public AuthorizationModelBinderTestsFixture SetAuthorizationContext()
        {
            AuthorizationContext.Set(nameof(UserRef), UserRef);

            return this;
        }
    }
}
#endif