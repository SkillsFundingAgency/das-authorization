using System.Collections.Concurrent;
using System.Linq;

namespace SFA.DAS.Authorization.Features
{
    public class FeatureTogglesService<TConfiguration, TFeatureToggle> : IFeatureTogglesService<TFeatureToggle>
        where TConfiguration : IFeaturesConfiguration<TFeatureToggle>, new()
        where TFeatureToggle : FeatureToggle, new()
    {
        private readonly ConcurrentDictionary<string, TFeatureToggle> _featureToggles;

        public FeatureTogglesService(TConfiguration configuration)
        {
            _featureToggles = new ConcurrentDictionary<string, TFeatureToggle>(configuration.FeatureToggles.ToDictionary(t => t.Feature));
        }

        public TFeatureToggle GetFeatureToggle(string feature)
        {
            return _featureToggles.GetOrAdd(feature, f => new TFeatureToggle { Feature = f, IsEnabled = false });
        }
    }
}