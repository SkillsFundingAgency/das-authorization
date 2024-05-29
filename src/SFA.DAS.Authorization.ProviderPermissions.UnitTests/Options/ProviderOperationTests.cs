using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Authorization.ProviderPermissions.Options;
using SFA.DAS.Authorization.ProviderPermissions.Types;


namespace SFA.DAS.Authorization.ProviderPermissions.UnitTests.Options
{
    [TestFixture]
    [Parallelizable]
    public class ProviderOperationTests
    {
        /// <remarks>
        /// All ProviderOperation const names must match a value's name in the api client's Operation enum
        /// (but we don't need a const name for every enum value)
        /// </remarks>
        [Test]
        public void EveryProviderOperationNameMustMatchAProviderRelationshipsOperationEnumValue()
        {
            var providerOperations = typeof(ProviderOperation)
                .GetFields()
                .Select(f => f.GetRawConstantValue())
                .Cast<string>()
                .Select(o => o.Substring(ProviderOperation.Prefix.Length));

            foreach (var providerOperation in providerOperations)
            {
                Enum.Parse(typeof(Operation), providerOperation);
            }
        }
    }
}