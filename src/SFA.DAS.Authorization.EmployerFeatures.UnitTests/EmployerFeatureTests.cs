using System;
using System.Linq;
using NUnit.Framework;

namespace SFA.DAS.Authorization.EmployerFeatures.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class EmployerFeatureTests
    {
        /// <remarks>
        /// All ProviderOperation const names must match a value's name in the api client's Operation enum
        /// (but we don't need a const name for every enum value)
        /// </remarks>
        [Test]
        public void EveryEmployerFeatureNameMustMatchAFeatureEnumValue()
        {
            var nakedOperationPos = EmployerFeature.Namespace.Length + 1;
            var providerOperations = typeof(EmployerFeature).GetFields().Select(f => f.GetRawConstantValue()).Cast<string>().Select(o => o.Substring(nakedOperationPos));

            foreach (var providerOperation in providerOperations)
            {
                Enum.Parse(typeof(Feature), providerOperation);
            }
        }
    }
}