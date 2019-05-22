using System.Collections.Generic;

namespace SFA.DAS.Authorization.Features
{
    public class FeaturesConfiguration : IFeaturesConfiguration<FeatureToggle>
    {
        public List<FeatureToggle> FeatureToggles { get; set; }
    }
}