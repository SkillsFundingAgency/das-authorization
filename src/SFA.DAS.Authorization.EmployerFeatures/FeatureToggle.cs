using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class FeatureToggle
    {
        public string Feature { get; }
        public bool IsEnabled { get; }
        public List<FeatureToggleWhitelistItem> Whitelist { get; }
        public bool IsWhitelistEnabled => Whitelist != null && Whitelist.Any();

        public FeatureToggle(string feature, bool isEnabled, List<FeatureToggleWhitelistItem> whitelist)
        {
            Feature = feature;
            IsEnabled = isEnabled;
            Whitelist = whitelist;
        }

        public bool IsUserWhitelisted(long accountId, string userEmail)
        {
            return Whitelist.Any(w => w.AccountId == accountId && (w.UserEmails == null || !w.UserEmails.Any() || w.UserEmails.Contains(userEmail, StringComparer.InvariantCultureIgnoreCase)));
        }
    }
}