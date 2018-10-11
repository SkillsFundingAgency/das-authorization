﻿#if NET462
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
    public class AuthorizationModelBinderTests : FluentTest<AuthorizationModelBinderTestsFixture>
    {
        [Test]
        public void BindModel_WhenBindingAnAuthorizationContextMessageAndAPropertyNameExistsInTheAuthorizationContext_ThenShouldSetThePropertyValue()
        {
            Run(f => f.SetAuthorizationContext(), f => f.BindModel(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationContextMessageStub>(m => m.UserRef == f.UserRef));
        }

        [Test]
        public void BindModel_WhenBindingAnAuthorizationContextMessageAndAPropertyNameDoesNotExistInTheAuthorizationContext_ThenShouldNotSetThePropertyValue()
        {
            Run(f => f.SetAuthorizationContext(), f => f.BindModel(), (f, r) => r.Should().NotBeNull().And.Match<AuthorizationContextMessageStub>(m => m.Foo == null));
        }
    }

    public class AuthorizationModelBinderTestsFixture
    {
        public Guid UserRef { get; set; }
        public ControllerContext ControllerContext { get; set; }
        public ModelBindingContext BindingContext { get; set; }
        public NameValueCollectionValueProvider ValueProvider { get; set; }
        public DefaultModelBinder ModelBinder { get; set; }
        public Mock<IAuthorizationContextProvider> AuthorizationContextProvider { get; set; }
        public AuthorizationContext AuthorizationContext { get; set; }

        public AuthorizationModelBinderTestsFixture()
        {
            UserRef = Guid.NewGuid();
            ControllerContext = new ControllerContext(Mock.Of<HttpContextBase>(), new RouteData(), Mock.Of<ControllerBase>());
            ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), null);

            BindingContext = new ModelBindingContext
            {
                ModelName = "",
                ValueProvider = ValueProvider,
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(AuthorizationContextMessageStub))
            };

            AuthorizationContextProvider = new Mock<IAuthorizationContextProvider>();
            AuthorizationContext = new AuthorizationContext();

            AuthorizationContextProvider.Setup(p => p.GetAuthorizationContext()).Returns(AuthorizationContext);

            ModelBinder = new AuthorizationModelBinder(() => AuthorizationContextProvider.Object);
        }

        public AuthorizationContextMessageStub BindModel()
        {
            return ModelBinder.BindModel(ControllerContext, BindingContext) as AuthorizationContextMessageStub;
        }

        public AuthorizationModelBinderTestsFixture SetAuthorizationContext()
        {
            AuthorizationContext.Set(nameof(UserRef), UserRef);

            return this;
        }
    }
}
#endif