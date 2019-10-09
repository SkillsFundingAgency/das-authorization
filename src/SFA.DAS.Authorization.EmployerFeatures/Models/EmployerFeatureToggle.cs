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

        public bool IsUserWhitelisted(long? accountId, string userEmail) =>
            IsAccountWhitelisted(accountId) || IsEmailWhitelisted(userEmail);

        private bool IsAccountWhitelisted(long? accountId)
            => accountId.HasValue && AccountWhitelist.Contains(accountId.Value);

        private bool IsEmailWhitelisted(string userEmail)
        {
            var wildcards = EmailWhitelist.Where(s => s.Contains("*"));
            var emails = EmailWhitelist.Except(wildcards);

            if(string.IsNullOrWhiteSpace(userEmail))
            {
                return false;
            }

            return emails.Any(email => email.Equals(userEmail, StringComparison.InvariantCultureIgnoreCase))
                    || wildcards.Any(pattern => Regex.IsMatch(userEmail, pattern, RegexOptions.IgnoreCase));
        }
    }
}