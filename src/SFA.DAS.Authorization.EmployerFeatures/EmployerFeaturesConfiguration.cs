using System.Collections.Generic;
using SFA.DAS.Authorization.Features;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesConfiguration : IFeaturesConfiguration<EmployerFeatureToggle>
    {
        public List<EmployerFeatureToggle> FeatureToggles { get; set; }
    }
}