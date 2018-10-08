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
            Run(f => f.GetData(), (f, a) => a.Should().Throw<KeyNotFoundException>().WithMessage($"The key '{f.Key}' was not present in the dictionary"));
        }
    }

    public class AuthorizationContextTestsFixture
    {
        public string Key { get; set; }
        public object Data { get; set; }
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
    }
}