using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationContextTests : FluentTest<AuthorizationContextTestsFixture>
    {
        [Test]
        public void Get_WhenKeyExists_ThenShouldReturnData()
        {
            Test(f => f.SetData(), f => f.GetData(), (f, d) => d.Should().Be(f.Data));
        }

        [Test]
        public void Get_WhenKeyDoesNotExist_ThenShouldThrowException()
        {
            TestException(f => f.GetData(), (f, r) => r.Should().Throw<KeyNotFoundException>().WithMessage($"The key '{f.Key}' was not present in the authorization context"));
        }

        [Test]
        public void TryGet_WhenKeyExists_ThenShouldReturnTrueAndValueShouldNotBeNull()
        {
            Test(f => f.SetData(), f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeTrue();
                f.Value.Should().NotBeNull().And.Be(f.Data);
            });
        }

        [Test]
        public void TryGet_WhenKeyDoesNotExist_ThenShouldReturnFalseAndValueShouldBeNull()
        {
            Test(f => f.TryGetData(), (f, r) =>
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
            AuthorizationContext.Set(Key, Data);

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