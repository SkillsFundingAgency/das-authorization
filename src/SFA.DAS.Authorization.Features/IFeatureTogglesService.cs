namespace SFA.DAS.Authorization.Features
{
    public interface IFeatureTogglesService<T> where T : FeatureToggle, new()
    {
        T GetFeatureToggle(string feature);
    }
}