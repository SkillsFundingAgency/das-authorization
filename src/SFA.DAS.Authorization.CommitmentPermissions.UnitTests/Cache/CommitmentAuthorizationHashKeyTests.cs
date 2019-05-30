using System;
using NUnit.Framework;
using SFA.DAS.Authorization.CommitmentPermissions.Cache;

namespace SFA.DAS.Authorization.CommitmentPermissions.UnitTests.Cache
{
    [TestFixture]
    public class CommitmentAuthorizationHashKeyTests
    {
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2,Option3", true)]
        [TestCase("1:1:1:Option1,Option2,Option3", "2:1:1:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:2:1:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:3:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2,Option3,Option4", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option3,Option2,Option1", false)]

        public void GetHashCode_ComparingTwoObjects_MightReturnDifferentHashes(string s1, string s2, bool expectToBeTheSame)
        {
            // Note: should be the same for equal objects but might still be the same for completely different objects.
            //       The false cases have been selected because they are known to have different hashes. 
            var key1 = FromString(s1);
            var key2 = FromString(s2);

            CheckHash(key1, key2, expectToBeTheSame);
        }

        [TestCase("1:1:1:Option1,Option2,Option3")]
        public void GetHashCode_ComparingTheSameInstance_ShouldReturnTrue(string s1)
        {
            var key1 = FromString(s1);

            CheckHash(key1, key1, true);
        }

        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2,Option3", true)]
        [TestCase("1:1:1:Option1,Option2,Option3", "2:1:1:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:2:1:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:3:Option1,Option2,Option3", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2,Option3,Option4", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option1,Option2", false)]
        [TestCase("1:1:1:Option1,Option2,Option3", "1:1:1:Option3,Option2,Option1", false)]

        public void Equals_ComparingTwoObjects_ShouldReturnCorrectValue(string s1, string s2, bool expectToBeTheSame)
        {
            // Note: should be the same for equal objects but might still be the same for completely different objects.
            var key1 = FromString(s1);
            var key2 = FromString(s2);

            CheckEquality(key1, key2, expectToBeTheSame);
            CheckEqualityOperator(key1, key2, expectToBeTheSame);
        }

        private void CheckHash(CommitmentAuthorizationHashKey k1, CommitmentAuthorizationHashKey k2, bool expectToBeTheSame)
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

        private void CheckEquality(CommitmentAuthorizationHashKey k1, CommitmentAuthorizationHashKey k2, bool expectToBeTheSame)
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

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void CheckEqualityOperator(CommitmentAuthorizationHashKey k1, CommitmentAuthorizationHashKey k2, bool expectToBeTheSame)
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

        private CommitmentAuthorizationHashKey FromString(string s)
        {
            var parts = s.Split(new []{':'}, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 4)
            {
                throw new InvalidOperationException($"The test string should be in the format \"<party-type>:<party-id>:<cohort-id>:<option-1>,<option-2>...<option-n>\"");
            }

            return new CommitmentAuthorizationHashKey(
                (PartyType)GetAsInt(parts,0), 
                GetAsInt(parts, 1), 
            GetAsInt(parts, 2), 
                parts[3].Split(new []{','}, StringSplitOptions.RemoveEmptyEntries));
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
