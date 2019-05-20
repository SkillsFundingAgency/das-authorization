using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Authorization.ProviderFeatures
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

        public bool IsUserWhitelisted(long ukprn, string userEmail)
        {
            return Whitelist.Any(w => w.Ukprn == ukprn && (w.UserEmails == null || !w.UserEmails.Any() || w.UserEmails.Contains(userEmail, StringComparer.InvariantCultureIgnoreCase)));
        }
    }
}