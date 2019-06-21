using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Authorization.Features.Models;

namespace SFA.DAS.Authorization.EmployerFeatures.Models
{
    public class EmployerFeatureToggle : FeatureToggle
    {
        public List<EmployerFeatureToggleWhitelistItem> Whitelist { get; set; }
        public bool IsWhitelistEnabled => Whitelist != null && Whitelist.Count > 0;

        public bool IsUserWhitelisted(long accountId, string userEmail)
        {
            return Whitelist.Any(w => w.AccountId == accountId && (w.UserEmails == null || w.UserEmails.Count == 0 || w.UserEmails.Contains(userEmail, StringComparer.InvariantCultureIgnoreCase)));
        }
    }
}