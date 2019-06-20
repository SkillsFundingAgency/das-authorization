using SFA.DAS.Authorization.Features.Models;

namespace SFA.DAS.Authorization.Features.Services
{
    public interface IFeatureTogglesService<T> where T : FeatureToggle, new()
    {
        T GetFeatureToggle(string feature);
    }
}