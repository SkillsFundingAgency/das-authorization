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
            _featureToggles = Enum.GetValues(typeof(Feature)).Cast<Feature>().ToDictionary(f => f, f => 
                configuration.FeatureToggles.SingleOrDefault(t => t.Feature == f) ?? new FeatureToggle(f, false, new List<string>()));
        }

        public Task<FeatureToggle> GetFeatureToggle(Feature feature)
        {
            return Task.FromResult(_featureToggles[feature]);
        }
    }
}