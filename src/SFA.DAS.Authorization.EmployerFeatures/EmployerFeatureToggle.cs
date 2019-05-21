using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Authorization.Features;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeatureToggle : FeatureToggle
    {
        public List<EmployerFeatureToggleWhitelistItem> Whitelist { get; set; }
        public bool IsWhitelistEnabled => Whitelist != null && Whitelist.Any();

        public bool IsUserWhitelisted(long accountId, string userEmail)
        {
            return Whitelist.Any(w => w.AccountId == accountId && (w.UserEmails == null || !w.UserEmails.Any() || w.UserEmails.Contains(userEmail, StringComparer.InvariantCultureIgnoreCase)));
        }
    }
}