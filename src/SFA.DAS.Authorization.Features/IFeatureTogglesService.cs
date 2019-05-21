namespace SFA.DAS.Authorization.Features
{
    public interface IFeatureTogglesService
    {
        FeatureToggle GetFeatureToggle(string feature);
    }
}