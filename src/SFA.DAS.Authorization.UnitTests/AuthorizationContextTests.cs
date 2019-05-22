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
            Test(f => f.SetData(), f => f.GetData(), (f, d) => d.Should().Be(f.Value));
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
                f.ValueOut.Should().NotBeNull().And.Be(f.Value);
            });
        }

        [Test]
        public void TryGet_WhenKeyDoesNotExist_ThenShouldReturnFalseAndValueShouldBeNull()
        {
            Test(f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeFalse();
                f.ValueOut.Should().BeNull();
            });
        }

        [Test]
        public void ToString_WhenKeysExist_ThenShouldReturnAuthorizedDescription()
        {
            Test(f => f.SetData(3), f => f.AuthorizationContext.ToString(), (f, r) => r.Should().Be("Foo_0: Bar_0, Foo_1: Bar_1, Foo_2: Bar_2"));
        }

        [Test]
        public void ToString_WhenKeysDoNotExist_ThenShouldReturnUnauthorizedDescription()
        {
            Test(f => f.AuthorizationContext.ToString(), (f, r) => r.Should().Be("None"));
        }
    }

    public class AuthorizationContextTestsFixture
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string ValueOut { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }

        public AuthorizationContextTestsFixture()
        {
            Key = "Foo";
            Value = "Bar";
            AuthorizationContext = new AuthorizationContext();
        }

        public string GetData()
        {
            return AuthorizationContext.Get<string>(Key);
        }

        public AuthorizationContextTestsFixture SetData(int count = 1)
        {
            if (count == 1)
            {
                AuthorizationContext.Set(Key, Value);
            }
            else
            {
                for (var i = 0; i < count; i++)
                {
                    AuthorizationContext.Set($"{Key}_{i}", $"{Value}_{i}");
                }
            }

            return this;
        }

        public bool TryGetData()
        {
            var exists = AuthorizationContext.TryGet(Key, out string value);

            ValueOut = value;

            return exists;
        }
    }
}