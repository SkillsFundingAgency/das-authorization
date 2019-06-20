using System.Collections.Generic;
using SFA.DAS.Authorization.Features.Models;

namespace SFA.DAS.Authorization.Features.Configuration
{
    public interface IFeaturesConfiguration<T> where T : FeatureToggle, new()
    {
        List<T> FeatureToggles { get; }
    }
}