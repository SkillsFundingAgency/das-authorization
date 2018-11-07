using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.Authorization.ProviderPermissions.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class ProviderOperationTests
    {
        /// <remarks>
        /// All EmployerFeature const names must match a value's name in the Feature enum
        /// (but we don't need a const name for every enum value)
        /// </remarks>
        [Test]
        public void EveryProviderOperationNameMustMatchAProviderRelationshipsApiOperationEnumValue()
        {
            var nakedOperationPos = ProviderOperation.Namespace.Length + 1;
            var employerFeatures = typeof(ProviderOperation).GetFields().Select(f => f.GetRawConstantValue()).Cast<string>().Select(o => o.Substring(nakedOperationPos));

            foreach (var providerOperation in employerFeatures)
            {
                Enum.Parse(typeof(Operation), providerOperation);
            }
        }
    }
}