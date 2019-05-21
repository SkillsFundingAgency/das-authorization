using System.Collections.Generic;

namespace SFA.DAS.Authorization.Features
{
    public interface IFeaturesConfiguration<T> where T : FeatureToggle, new()
    {
        List<T> FeatureToggles { get; }
    }
}