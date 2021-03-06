using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.EmployerAccounts.Types.Models;

namespace SFA.DAS.Authorization.EmployerUserRoles.UnitTests.Options
{
    [TestFixture]
    [Parallelizable]
    public class EmployerUserRoleTests
    {
        /// <remarks>
        /// All EmployerUserRole const names must match a value's name in the api client's UserRole enum
        /// (but we don't need a const name for every enum value)
        /// </remarks>
        [Test]
        public void EveryEmployerUserRoleNameMustMatchAEmployerAccountsUserRoleEnumValue()
        {
            var employerUserRoles = typeof(EmployerUserRole)
                .GetFields()
                .Select(f => f.GetRawConstantValue())
                .Cast<string>()
                .Where(o => o != EmployerUserRole.Any)
                .SelectMany(o => o.Split(','))
                .Select(o => o.Substring(EmployerUserRole.Prefix.Length));

            foreach (var employerUserRole in employerUserRoles)
            {
                Enum.Parse(typeof(UserRole), employerUserRole);
            }
        }
    }
}