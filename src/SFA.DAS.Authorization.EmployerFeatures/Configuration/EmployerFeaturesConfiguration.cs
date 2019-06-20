using System.Collections.Generic;
using SFA.DAS.Authorization.Features.Configuration;

namespace SFA.DAS.Authorization.EmployerFeatures.Configuration
{
    public class EmployerFeaturesConfiguration : IFeaturesConfiguration<EmployerFeatureToggle>
    {
        public List<EmployerFeatureToggle> FeatureToggles { get; set; }
    }
}