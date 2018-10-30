#if NET462
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Hosting;
using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using System.Web.Http.ValueProviders.Providers;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.WebApi.UnitTests
{
    [TestFixture]
    public class AuthorizationModelBinderTests : FluentTest<AuthorizationModelBinderTestsFixture>
    {
        [Test]
        public void BindModel_WhenBindingAnAuthorizationContextMessageAndAPropertyNameExistsInTheAuthorizationContext_ThenShouldSetThePropertyValue()
        {
            Run(f => f.SetAuthorizationContext(), f => f.BindModel(), (f, r) =>
            {
                r.Should().BeTrue();
                f.BindingContext.Model.Should().Be(f.UserRef);
            });
        }

        [Test]
        public void BindModel_WhenBindingAnAuthorizationContextMessageAndAPropertyNameDoesNotExistInTheAuthorizationContext_ThenShouldNotSetThePropertyValue()
        {
            Run(f => f.BindModel(), (f, r) =>
            {
                r.Should().BeFalse();
                f.BindingContext.Model.Should().BeNull();
            });
        }
    }

    public class AuthorizationModelBinderTestsFixture
    {
        public Guid UserRef { get; set; }
        public HttpRequestMessage HttpRequestMessage { get; set; }
        public HttpControllerContext ControllerContext { get; set; }
        public Mock<HttpActionDescriptor> ActionDescriptor { get; set; }
        public Mock<IDependencyScope> DependencyScope { get; set; }
        public HttpActionContext ActionContext { get; set; }
        public ModelMetadataProvider MetadataProvider { get; set; }
        public IValueProvider ValueProvider { get; set; }
        public ModelBindingContext BindingContext { get; set; }
        public IModelBinder ModelBinder { get; set; }
        public Mock<IAuthorizationContextProvider> AuthorizationContextProvider { get; set; }
        public AuthorizationContext AuthorizationContext { get; set; }

        public AuthorizationModelBinderTestsFixture()
        {
            UserRef = Guid.NewGuid();
            HttpRequestMessage = new HttpRequestMessage();
            ControllerContext = new HttpControllerContext { Request = HttpRequestMessage };
            ActionDescriptor = new Mock<HttpActionDescriptor>();
            DependencyScope = new Mock<IDependencyScope>();
            ActionContext = new HttpActionContext(ControllerContext, ActionDescriptor.Object);
            MetadataProvider = new DataAnnotationsModelMetadataProvider();
            ValueProvider = new NameValuePairsValueProvider(new Dictionary<string, string>(), null);

            BindingContext = new ModelBindingContext
            {
                ModelMetadata = MetadataProvider.GetMetadataForProperty(null, typeof(AuthorizationContextMessageStub), nameof(UserRef)),
                ModelName = "",
                ValueProvider = ValueProvider
            };

            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContext = new AuthorizationContext();

            DependencyScope.Setup(s => s.GetService(typeof(IAuthorizationContextProvider))).Returns(AuthorizationContextProvider.Object);
            HttpRequestMessage.Properties[HttpPropertyKeys.DependencyScope] = DependencyScope.Object;
            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(AuthorizationContext);

            ModelBinder = new AuthorizationModelBinder();
        }

        public bool BindModel()
        {
            return ModelBinder.BindModel(ActionContext, BindingContext);
        }

        public AuthorizationModelBinderTestsFixture SetAuthorizationContext()
        {
            AuthorizationContext.Add(nameof(UserRef), UserRef);

            return this;
        }
    }
}
#endif