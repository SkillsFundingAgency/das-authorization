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
        /// All EmployerFeature const names must match a value's name in the Feature enum
        /// (but we don't need a const name for every enum value)
        /// </remarks>
        [Test]
        public void EveryEmployerFeatureNameMustMatchAFeatureEnumValue()
        {
            var nakedEmployerFeaturePos = EmployerFeature.Namespace.Length + 1;
            
            var employerFeatures = typeof(EmployerFeature)
                .GetFields()
                .Select(f => f.GetRawConstantValue())
                .Cast<string>()
                .Select(o => o.Substring(nakedEmployerFeaturePos));

            foreach (var employerFeature in employerFeatures)
            {
                Enum.Parse(typeof(Feature), employerFeature);
            }
        }
    }
}