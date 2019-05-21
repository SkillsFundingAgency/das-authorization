using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Authorization.Features;

namespace SFA.DAS.Authorization.ProviderFeatures
{
    public class ProviderFeatureToggle : FeatureToggle
    {
        public List<ProviderFeatureToggleWhitelistItem> Whitelist { get; set; }
        public bool IsWhitelistEnabled => Whitelist != null && Whitelist.Any();

        public bool IsUserWhitelisted(long ukprn, string userEmail)
        {
            return Whitelist.Any(w => w.Ukprn == ukprn && (w.UserEmails == null || !w.UserEmails.Any() || w.UserEmails.Contains(userEmail, StringComparer.InvariantCultureIgnoreCase)));
        }
    }
}