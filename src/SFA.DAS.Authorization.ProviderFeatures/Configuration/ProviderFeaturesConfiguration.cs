using System.Collections.Generic;
using SFA.DAS.Authorization.Features.Configuration;
using SFA.DAS.Authorization.ProviderFeatures.Models;

namespace SFA.DAS.Authorization.ProviderFeatures.Configuration
{
    public class ProviderFeaturesConfiguration : IFeaturesConfiguration<ProviderFeatureToggle>
    {
        public List<ProviderFeatureToggle> FeatureToggles { get; set; }
    }
}