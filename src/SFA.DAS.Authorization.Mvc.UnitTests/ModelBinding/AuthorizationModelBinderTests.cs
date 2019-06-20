#if NETCOREAPP2_0
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Mvc.ModelBinding;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.Mvc.UnitTests.ModelBinding
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationModelBinderTests : FluentTest<AuthorizationModelBinderTestsFixture>
    {
        [Test]
        public Task BindModel_WhenBindingAnAuthorizationContextModelAndAPropertyNameExistsInTheAuthorizationContext_ThenShouldSetThePropertyValue()
        {
            return TestAsync(f => f.SetAuthorizationContext(f.UserRef), f => f.BindModel(), f =>
            {
                f.BindingContext.VerifySet(c => c.Result = It.Is<ModelBindingResult>(m => m.Model.Equals(f.UserRef) && m.IsModelSet));
            });
        }

        [Test]
        public Task BindModel_WhenBindingAnAuthorizationContextModelAndAPropertyNameExistsInTheAuthorizationContextButContextValueIsNull_ThenShouldSetThePropertyValueToNull()
        {
            return TestAsync(f => f.SetAuthorizationContext(null), f => f.BindModel(), f =>
            {
                f.BindingContext.VerifySet(c => c.Result = It.Is<ModelBindingResult>(m=>m.Model == null && m.IsModelSet));
            });
        }


        [Test]
        public Task BindModel_WhenBindingAnAuthorizationContextModelAndAPropertyNameDoesNotExistInTheAuthorizationContext_ThenShouldNotSetThePropertyValue()
        {
            return TestAsync(f => f.BindModel(), f =>
            {
                f.BindingContext.VerifySet(c => c.Result = It.IsAny<ModelBindingResult>(), Times.Never);
            });
        }

        [Test]
        public Task BindModel_WhenBindingAnAuthorizationContextModelAndAPropertyNameDoesNotExistInTheAuthorizationContext_ThenShouldCallFallbackModelBinder()
        {
            return TestAsync(f => f.BindModel(), f =>
            {
                f.FallbackModelBinder.Verify(b => b.BindModelAsync(f.BindingContext.Object), Times.Once);
            });
        }
    }

    public class AuthorizationModelBinderTestsFixture
    {
        public Guid? UserRef { get; set; }
        public ModelMetadataProvider ModelMetadataProvider { get; set; }
        public ModelMetadata ModelMetadata { get; set; }
        public ModelStateDictionary ModelState { get; set; }
        public Mock<ModelBindingContext> BindingContext { get; set; }
        public IModelBinder ModelBinder { get; set; }
        public Mock<IModelBinder> FallbackModelBinder { get; set; }
        public Mock<IAuthorizationContextProvider> AuthorizationContextProvider { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }

        public AuthorizationModelBinderTestsFixture()
        {
            UserRef = Guid.NewGuid();

            ModelMetadataProvider = new EmptyModelMetadataProvider();
            ModelMetadata = ModelMetadataProvider.GetMetadataForProperty(typeof(AuthorizationContextModelStub), nameof(AuthorizationContextModelStub.UserRef));
            ModelState = new ModelStateDictionary();
            BindingContext = new Mock<ModelBindingContext>();
            FallbackModelBinder = new Mock<IModelBinder>();
            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContext = new AuthorizationContext();

            BindingContext.Setup(c => c.HttpContext.RequestServices.GetService(typeof(IAuthorizationContextProvider))).Returns(AuthorizationContextProvider.Object);
            BindingContext.Setup(c => c.ModelMetadata).Returns(ModelMetadata);
            BindingContext.Setup(c => c.ModelState).Returns(ModelState);
            BindingContext.Setup(c => c.ModelName).Returns("");
            FallbackModelBinder.Setup(b => b.BindModelAsync(BindingContext.Object)).Returns(Task.CompletedTask);
            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(AuthorizationContext);

            ModelBinder = new AuthorizationModelBinder(FallbackModelBinder.Object);
        }

        public Task BindModel()
        {
            return ModelBinder.BindModelAsync(BindingContext.Object);
        }

        public AuthorizationModelBinderTestsFixture SetAuthorizationContext(Guid? userRef)
        {
            AuthorizationContext.Set(nameof(UserRef), userRef);

            return this;
        }
    }
}
#elif NET462
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Mvc.ModelBinding;
using SFA.DAS.Testing;
using AuthorizationContext = SFA.DAS.Authorization.Context.AuthorizationContext;

namespace SFA.DAS.Authorization.Mvc.UnitTests.ModelBinding
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationModelBinderTests : FluentTest<AuthorizationModelBinderTestsFixture>
    {
        [Test]
        public void BindModel_WhenBindingAnAuthorizationContextModelAndAPropertyNameExistsInTheAuthorizationContext_ThenShouldSetThePropertyValue()
        {
            Test(f => f.SetAuthorizationContext(f.UserRef), f => f.BindModel(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.UserRef.Should().Be(f.UserRef);
            });
        }

        [Test]
        public void BindModel_WhenBindingAnAuthorizationContextModelAndAPropertyNameExistsInTheAuthorizationContextButContextValueIsNull_ThenShouldSetThePropertyValueToNull()
        {
            Test(f => f.SetAuthorizationContext(null), f => f.BindModel(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.UserRef.Should().BeNull();
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
        public Guid? UserRef { get; set; }
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

        public AuthorizationModelBinderTestsFixture SetAuthorizationContext(Guid? value)
        {
            AuthorizationContext.Set(nameof(UserRef), value);

            return this;
        }
    }
}
#endif