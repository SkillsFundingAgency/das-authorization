using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Types.Models;

namespace SFA.DAS.Authorization.EmployerRoles.UnitTests
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
            var nakedEmployerUserRolePos = EmployerUserRole.Namespace.Length + 1;
            
            var employerUserRoles = typeof(EmployerUserRole)
                .GetFields()
                .Select(f => f.GetRawConstantValue())
                .Cast<string>()
                .Where(o => o != EmployerUserRole.Any)
                .SelectMany(o => o.Split(','))
                .Select(o => o.Substring(nakedEmployerUserRolePos));

            foreach (var employerUserRole in employerUserRoles)
            {
                Enum.Parse(typeof(UserRole), employerUserRole);
            }
        }
    }
}