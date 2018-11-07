using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public interface IFeatureTogglesService
    {
        Task<FeatureToggle> GetFeatureToggle(Feature feature);
    }
}