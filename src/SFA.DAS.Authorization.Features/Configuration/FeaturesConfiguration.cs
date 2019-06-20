using System.Collections.Generic;
using SFA.DAS.Authorization.Features.Models;

namespace SFA.DAS.Authorization.Features.Configuration
{
    public class FeaturesConfiguration : IFeaturesConfiguration<FeatureToggle>
    {
        public List<FeatureToggle> FeatureToggles { get; set; }
    }
}