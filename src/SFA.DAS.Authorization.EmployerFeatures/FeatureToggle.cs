using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class FeatureToggle
    {
        public virtual Feature Feature { get; protected set; }
        public virtual bool IsEnabled { get; protected set; }
        public virtual List<string> UserEmailWhitelist { get; protected set; } = new List<string>();

        public FeatureToggle(Feature feature, bool isEnabled, List<string> userEmailWhitelist)
        {
            Feature = feature;
            IsEnabled = isEnabled;
            UserEmailWhitelist = userEmailWhitelist;
        }

        public bool IsUserEmailWhitelisted(string userEmail)
        {
            return UserEmailWhitelist.Any() && UserEmailWhitelist.Any(e => e.Equals(userEmail, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}