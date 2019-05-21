using System.Collections.Concurrent;
using System.Linq;

namespace SFA.DAS.Authorization.Features
{
    public class FeatureTogglesService : IFeatureTogglesService
    {
        private readonly ConcurrentDictionary<string, FeatureToggle> _featureToggles;

        public FeatureTogglesService(FeaturesConfiguration configuration)
        {
            _featureToggles = new ConcurrentDictionary<string, FeatureToggle>(configuration.FeatureToggles.ToDictionary(t => t.Feature));
        }

        public FeatureToggle GetFeatureToggle(string feature)
        {
            return _featureToggles.GetOrAdd(feature, f => new FeatureToggle(f, false));
        }
    }
}