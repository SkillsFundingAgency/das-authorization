using System;
using NUnit.Framework;
using SFA.DAS.Authorization.CommitmentPermissions.Cache;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests.Cache
{
    [TestFixture]
    [Parallelizable]
    public class CacheKeyTests
    {
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2,Option3", true)]
        [TestCase("1:1:1:Option1,Option2,Option3", "2:1:1:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:2:1:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:3:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2,Option3,Option4", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option3,Option2,Option1", false)]

        public void GetHashCode_WhenComparingTwoObjects_ThenShouldReturnDifferentHashesAppropriately(string s1, string s2, bool expectToBeTheSame)
        {
            // Note: Should be the same for equal objects but might still be the same for completely different objects.
            //       The false cases have been selected because they are known to have different hashes. 
            var key1 = FromString(s1);
            var key2 = FromString(s2);

            CheckHash(key1, key2, expectToBeTheSame);
        }

        [TestCase("1:1:1:Option1,Option2,Option3")]
        public void GetHashCode_WhenComparingTheSameInstance_ThenShouldReturnTrue(string s1)
        {
            var key = FromString(s1);

            CheckHash(key, key, true);
        }

        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2,Option3", true)]
        [TestCase("1:1:1:Option1,Option2,Option3", "2:1:1:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:2:1:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:3:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2,Option3,Option4", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option3,Option2,Option1", false)]

        public void Equals_WhenComparingTwoObjects_ThenShouldReturnCorrectValue(string s1, string s2, bool expectToBeTheSame)
        {
            // Note: Should be the same for equal objects but might still be the same for completely different objects.
            var key1 = FromString(s1);
            var key2 = FromString(s2);

            CheckEquality(key1, key2, expectToBeTheSame);
            CheckEqualityOperator(key1, key2, expectToBeTheSame);
        }

        private void CheckHash(CacheKey k1, CacheKey k2, bool expectToBeTheSame)
        {
            var hash1 = k1.GetHashCode();
            var hash2 = k2.GetHashCode();

            if (expectToBeTheSame)
            {
                Assert.AreEqual(hash1, hash2);
            }
            else
            {
                Assert.AreNotEqual(hash1, hash2);
            }
        }

        private void CheckEquality(CacheKey k1, CacheKey k2, bool expectToBeTheSame)
        {
            if (expectToBeTheSame)
            {
                Assert.AreEqual(k1, k2);
            }
            else
            {
                Assert.AreNotEqual(k1, k2);
            }
        }

        private void CheckEqualityOperator(CacheKey k1, CacheKey k2, bool expectToBeTheSame)
        {
            if (expectToBeTheSame)
            {
                Assert.IsTrue(k1 == k2);
                Assert.IsFalse(k1 != k2);
            }
            else
            {
                Assert.IsTrue(k1 != k2);
                Assert.IsFalse(k1 == k2);
            }
        }

        private CacheKey FromString(string s)
        {
            var parts = s.Split(new []{':'}, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 4)
            {
                throw new InvalidOperationException($"The test string should be in the format \"<party-type>:<party-id>:<cohort-id>:<option-1>,<option-2>...<option-n>\"");
            }

            var options = parts[3].Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);
            var cohortId = GetAsInt(parts, 2);
            var party = (Party)GetAsInt(parts, 0);
            var partyId = GetAsInt(parts, 1);
            var authorizationContext = new AuthorizationContext();
            
            authorizationContext.AddCommitmentPermissionValues(cohortId, party, partyId);
            
            return new CacheKey(options, authorizationContext);
        }

        private int GetAsInt(string[] options, int index)
        {
            if (!int.TryParse(options[index], out int result))
            {
                throw new InvalidOperationException($"The string {options[index]} should be an integer");
            }

            return result;
        }
    }
}