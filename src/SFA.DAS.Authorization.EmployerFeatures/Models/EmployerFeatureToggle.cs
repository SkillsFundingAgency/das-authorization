using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SFA.DAS.Authorization.Features.Models;

namespace SFA.DAS.Authorization.EmployerFeatures.Models
{
    public class EmployerFeatureToggle : FeatureToggle
    {
        public List<long> AccountWhitelist { get; set; } = new List<long>();
        public List<string> EmailWhitelist { get; set; } = new List<string>();
        public bool IsWhitelistEnabled =>
            (AccountWhitelist != null && AccountWhitelist.Count > 0)
            ||
            (EmailWhitelist != null && EmailWhitelist.Count > 0);

        public bool IsUserWhitelisted(long? accountId, string userEmail)
        {
            return (accountId.HasValue && AccountWhitelist.Contains(accountId.Value))
                ||
                (!string.IsNullOrWhiteSpace(userEmail) && EmailWhitelist.Any(email => Regex.IsMatch(userEmail, email, RegexOptions.IgnoreCase)));

        }
    }
}