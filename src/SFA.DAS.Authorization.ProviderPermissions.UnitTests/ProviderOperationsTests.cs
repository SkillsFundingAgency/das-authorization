using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.Authorization.ProviderPermissions.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class ProviderOperationsTests
    {
        /// <remarks>
        /// All ProviderOperation const names must match a value's name in the api client's Operation enum
        /// (but we don't need a const name for every enum value)
        /// </remarks>
        [Test]
        public void EveryProviderOperationNameMustMatchAProviderRelationshipsApiOperationEnumValue()
        {
            var nakedOperationPos = ProviderOperation.Namespace.Length + 1;
            var providerOperations = typeof(ProviderOperation).GetFields().Select(f => f.GetRawConstantValue()).Cast<string>().Select(o => o.Substring(nakedOperationPos));

            foreach (var providerOperation in providerOperations)
                Enum.Parse(typeof(Operation), providerOperation);
        }
    }
}