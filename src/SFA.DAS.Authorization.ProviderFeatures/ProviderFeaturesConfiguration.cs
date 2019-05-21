using System.Collections.Generic;
using SFA.DAS.Authorization.Features;

namespace SFA.DAS.Authorization.ProviderFeatures
{
    public class ProviderFeaturesConfiguration : IFeaturesConfiguration<ProviderFeatureToggle>
    {
        public List<ProviderFeatureToggle> FeatureToggles { get; set; }
    }
}