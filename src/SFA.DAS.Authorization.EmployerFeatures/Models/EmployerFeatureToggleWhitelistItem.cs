using System.Collections.Generic;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeatureToggleWhitelistItem
    {
        public long AccountId { get; set; }
        public List<string> UserEmails { get; set; }
    }
}