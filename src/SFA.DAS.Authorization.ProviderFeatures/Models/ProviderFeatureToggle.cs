using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Authorization.Features.Models;

namespace SFA.DAS.Authorization.ProviderFeatures.Models
{
    public class ProviderFeatureToggle : FeatureToggle
    {
        public List<ProviderFeatureToggleWhitelistItem> Whitelist { get; set; }
        public bool IsWhitelistEnabled => Whitelist != null && Whitelist.Count > 0;

        public bool IsUserWhitelisted(long ukprn, string userEmail)
        {
            return Whitelist.Any(w => w.Ukprn == ukprn && (w.UserEmails == null || w.UserEmails.Count == 0 || w.UserEmails.Contains(userEmail, StringComparer.InvariantCultureIgnoreCase)));
        }
    }
}