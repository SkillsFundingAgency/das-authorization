namespace SFA.DAS.Authorization.EmployerFeatures
{
    public interface IFeatureTogglesService
    {
        FeatureToggle GetFeatureToggle(Feature feature);
    }
}