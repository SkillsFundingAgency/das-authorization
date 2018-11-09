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
        public Task GetFeatureToggle_WhenFeatureToggleExistsForFeature_ThenShouldReturnFeatureToggle()
        {
            return RunAsync(f => f.SetFeatureToggle(), f => f.GetFeatureToggle(), (f, r) => r.Should().NotBeNull().And.BeSameAs(f.FeatureToggle));
        }
        
        [Test]
        public Task GetFeatureToggle_WhenFeatureToggleDoesNotExistForFeature_ThenShouldReturnDisabledFeatureToggle()
        {
            return RunAsync(f => f.GetFeatureToggle(), (f, r) => r.Should().NotBeNull().And.Match<FeatureToggle>(t => t.Feature == f.Feature && t.IsEnabled == false));
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
            FeatureToggles = new List<FeatureToggle>();
            EmployerFeaturesConfiguration = new EmployerFeaturesConfiguration { FeatureToggles = new List<FeatureToggle>() };
        }

        public Task<FeatureToggle> GetFeatureToggle()
        {
            FeatureToggleService = new FeatureTogglesService(EmployerFeaturesConfiguration);
            
            return FeatureToggleService.GetFeatureToggle(Feature);
        }

        public FeatureTogglesServiceTestsFixture SetFeatureToggle()
        {
            FeatureToggle = new FeatureToggle(Feature, true, new List<string> { "foo@bar.com" });
            FeatureToggles.Add(FeatureToggle);
            EmployerFeaturesConfiguration.FeatureToggles.Add(FeatureToggle);
            
            return this;
        }
    }
}