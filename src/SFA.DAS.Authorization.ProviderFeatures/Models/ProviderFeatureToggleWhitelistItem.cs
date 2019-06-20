using System.Collections.Generic;

namespace SFA.DAS.Authorization.ProviderFeatures.Models
{
    public class ProviderFeatureToggleWhitelistItem
    {
        public long Ukprn { get; set; }
        public List<string> UserEmails { get; set; }
    }
}