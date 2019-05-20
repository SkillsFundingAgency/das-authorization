using System.Collections.Generic;

namespace SFA.DAS.Authorization.ProviderFeatures
{
    public class FeatureToggleWhitelistItem
    {
        public long Ukprn { get; }
        public List<string> UserEmails { get; }

        public FeatureToggleWhitelistItem(long ukprn, List<string> userEmails)
        {
            Ukprn = ukprn;
            UserEmails = userEmails;
        }
    }
}