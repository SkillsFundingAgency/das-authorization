using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.UnitTests
{
    [TestFixture]
    public class AuthorizationContextTests : FluentTest<AuthorizationContextTestsFixture>
    {
        [Test]
        public void Get_WhenGettingData_ThenShouldReturnData()
        {
            Run(f => f.SetData(), f => f.GetData(), (f, d) => d.Should().Be(f.Data));
        }

        [Test]
        public void Get_WhenGettingDataAndKeyDoesNotExist_ThenShouldThrowException()
        {
            Run(f => f.GetData(), (f, r) => r.Should().Throw<KeyNotFoundException>().WithMessage($"The key '{f.Key}' was not present in the authorization context"));
        }

        [Test]
        public void TryGet_WhenTryingToGetDataAndKeyDoesExist_ThenShouldReturnTrueAndValueShouldNotBeNull()
        {
            Run(f => f.SetData(), f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeTrue();
                f.Value.Should().NotBeNull().And.Be(f.Data);
            });
        }

        [Test]
        public void TryGet_WhenTryingToGetDataAndKeyDoesNotExist_ThenShouldReturnFalseAndValueShouldBeNull()
        {
            Run(f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeFalse();
                f.Value.Should().BeNull();
            });
        }
    }

    public class AuthorizationContextTestsFixture
    {
        public string Key { get; set; }
        public object Data { get; set; }
        public object Value { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }

        public AuthorizationContextTestsFixture()
        {
            Key = "Foo";
            Data = new object();
            AuthorizationContext = new AuthorizationContext();
        }

        public object GetData()
        {
            return AuthorizationContext.Get<object>(Key);
        }

        public AuthorizationContextTestsFixture SetData()
        {
            AuthorizationContext.Add(Key, Data);

            return this;
        }

        public bool TryGetData()
        {
            var exists = AuthorizationContext.TryGet(Key, out object value);

            Value = value;

            return exists;
        }
    }
}