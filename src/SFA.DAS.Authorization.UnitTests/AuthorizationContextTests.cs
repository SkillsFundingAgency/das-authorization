using System;
using System.Linq;
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
        #region Get

        [Test]
        public void Get_WhenGettingDataAndDataIsReferenceType_ThenShouldReturnData()
        {
            Run(f => f.AddData(), f => f.GetData(), (f, d) => d.Should().Be(f.Data));
        }

        [Test]
        public void Get_WhenGettingDataAndDataIsValueType_ThenShouldReturnData()
        {
            const long data = 1L;
            Run(f => f.AddData(data), f => f.GetData(), (f, d) => d.Should().Be(data));
        }

        [Test]
        public void Get_WhenGettingDataAndDataIsNullable_ThenShouldReturnData()
        {
            long? data = 1L;
            Run(f => f.AddData(data), f => f.GetData(), (f, d) => d.Should().Be(data));
        }

        [Test]
        public void Get_WhenGettingDataAndDataIsNullableAndNull_ThenShouldThrowException()
        {
            long? data = null;
            Run(f => f.AddData(data), f => f.GetData(), (f, r) => r.Should().Throw<ArgumentNullException>().WithMessage($"The key '{f.Key}' was present in the authorization context but its value was null*"));
        }

        [Test]
        public void Get_WhenGettingDataButKeyDoesNotExist_ThenShouldThrowException()
        {
            Run(f => f.GetData(), (f, r) => r.Should().Throw<KeyNotFoundException>().WithMessage($"The key '{f.Key}' was not present in the authorization context"));
        }
        
        [Test]
        public void Get_WhenGettingDataButDataIsNull_ThenShouldThrowException()
        {
            // alternatively: f.SetData(null).AddData(), or f.SetNullData().AddData()
            Run(f => f.AddData(null), f => f.GetData(), (f, r) => r.Should().Throw<ArgumentNullException>().WithMessage($"The key '{f.Key}' was present in the authorization context but its value was null*"));
        }

        #endregion Get

        #region TryGet

        [Test]
        public void TryGet_WhenTryingToGetDataAndDataIsReferenceTypeAndKeyDoesExist_ThenShouldReturnTrueAndCorrectValueShouldBeReturned()
        {
            Run(f => f.AddData(), f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeTrue();
                f.Value.Should().NotBeNull().And.Be(f.Data);
            });
        }

        [Test]
        public void TryGet_WhenTryingToGetDataAndDataIsValueTypeAndKeyDoesExist_ThenShouldReturnTrueAndCorrectValueShouldBeReturned()
        {
            const long data = 1L;
            Run(f => f.AddData(data), f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeTrue();
                f.Value.Should().NotBeNull().And.Be(data);
            });
        }

        [Test]
        public void TryGet_WhenTryingToGetDataAndDataIsNullableAndKeyDoesExist_ThenShouldReturnTrueAndCorrectValueShouldBeReturned()
        {
            long? data = 1L;
            Run(f => f.AddData(data), f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeTrue();
                f.Value.Should().NotBeNull().And.Be(data);
            });
        }

        [Test]
        public void TryGet_WhenTryingToGetDataAndDataIsNullableAndNullAndKeyDoesExist_ThenShouldReturnTrueAndValueShouldBeNull()
        {
            long? data = null;
            Run(f => f.AddData(data), f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeTrue();
                f.Value.Should().BeNull();
            });
        }

        [Test]
        public void TryGet_WhenTryingToGetDataAndKeyDoesExistAndDataIsNull_ThenShouldReturnTrueAndValueShouldBeNull()
        {
            Run(f => f.AddData(null), f => f.TryGetData(), (f, r) =>
            {
                r.Should().BeTrue();
                f.Value.Should().BeNull();
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
        
        #endregion TryGet
        
        #region Add
        //todo: can we test Add() individually?
        #endregion Add
        
        #region Enumeration
        
        [Test]
        public void Get_WhenEnumeratingData_ThenShouldReturnEnumeratedData()
        {
            Run(f => f.AddData(), f => f.GetEnumeration(), (f, e) => e.Should().BeEquivalentTo(new KeyValuePair<string,object>(f.Key,f.Data)));
        }

        #endregion Enumeration
    }

    public class AuthorizationContextTestsFixture
    {
        public string Key { get; set; }
        public object Data { get; set; }
        public object Value { get; set; }
        public AuthorizationContext AuthorizationContext { get; set; }

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
        
        public AuthorizationContextTestsFixture AddData()
        {
            //todo: can we setup without relying on code under test's Add() working?? add internal c'tor that accepts dictionary and make internals visible to test project (sounds yuck!)
            AuthorizationContext.Add(Key, Data);
            return this;
        }

        public AuthorizationContextTestsFixture AddData(object data)
        {
            AuthorizationContext.Add(Key, data);
            return this;
        }
        
        public bool TryGetData()
        {
            var exists = AuthorizationContext.TryGet(Key, out object value);

            Value = value;

            return exists;
        }

        public IEnumerable<KeyValuePair<string, object>> GetEnumeration()
        {
            return AuthorizationContext.Select(kvp => kvp);
        }
    }
}