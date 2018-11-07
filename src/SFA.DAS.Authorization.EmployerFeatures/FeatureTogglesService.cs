using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class FeatureTogglesService : IFeatureTogglesService
    {
        private readonly Dictionary<Feature, FeatureToggle> _featureToggles;

        public FeatureTogglesService(EmployerFeaturesConfiguration configuration)
        {
            _featureToggles = Enum.GetValues(typeof(Feature)).Cast<Feature>().ToDictionary(f => f, f => configuration.FeatureToggles.Single(t => t.Feature == f));
        }

        public Task<FeatureToggle> GetFeatureToggle(Feature feature)
        {
            return Task.FromResult(_featureToggles[feature]);
        }
    }
}