namespace SFA.DAS.Authorization.EmployerFeatures
{
    public interface IFeatureTogglesService
    {
        FeatureToggle GetFeatureToggle(string feature);
    }
}