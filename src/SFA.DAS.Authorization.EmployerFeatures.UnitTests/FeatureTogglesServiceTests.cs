using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.Authorization.EmployerFeatures.UnitTests
{
    [TestFixture]
    public class FeatureTogglesServiceTests : FluentTest<FeatureTogglesServiceTestsFixture>
    {
        [Test]
        public Task GetFeatureToggle_WhenGettingFeatureToggle_ThenShouldReturnFeatureToggle()
        {
            return RunAsync(f => f.GetFeatureToggle(), (f, r) => r.Should().NotBeNull().And.BeSameAs(f.FeatureToggle));
        }
    }

    public class FeatureTogglesServiceTestsFixture
    {
        public Feature Feature { get; set; }
        public IFeatureTogglesService FeatureToggleService { get; set; }
        public EmployerFeaturesConfiguration EmployerFeaturesConfiguration { get; set; }
        public List<FeatureToggle> FeatureToggles { get; set; }
        public FeatureToggle FeatureToggle { get; set; }
        
        public FeatureTogglesServiceTestsFixture()
        {
            Feature = Feature.ProviderRelationships;
            FeatureToggle = new FeatureToggle(Feature, false, new List<string>());
            FeatureToggles = new List<FeatureToggle> { FeatureToggle };
            EmployerFeaturesConfiguration = new EmployerFeaturesConfiguration { FeatureToggles = FeatureToggles };
            FeatureToggleService = new FeatureTogglesService(EmployerFeaturesConfiguration);
        }

        public Task<FeatureToggle> GetFeatureToggle()
        {
            return FeatureToggleService.GetFeatureToggle(Feature);
        }
    }
}