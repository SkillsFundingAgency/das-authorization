namespace SFA.DAS.Authorization.ProviderFeatures
{
    public interface IFeatureTogglesService
    {
        FeatureToggle GetFeatureToggle(string feature);
    }
}