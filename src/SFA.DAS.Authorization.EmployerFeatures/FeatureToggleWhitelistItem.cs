using System.Collections.Generic;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class FeatureToggleWhitelistItem
    {
        public long AccountId { get; }
        public List<string> UserEmails { get; }

        public FeatureToggleWhitelistItem(long accountId, List<string> userEmails)
        {
            AccountId = accountId;
            UserEmails = userEmails;
        }
    }
}