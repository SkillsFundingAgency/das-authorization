using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Extensions;
using SFA.DAS.Authorization.Features.Errors;
using SFA.DAS.Authorization.Features.Models;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.Features.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => "Feature.";
        
        private readonly IFeatureTogglesService<FeatureToggle> _featureTogglesService;

        public AuthorizationHandler(IFeatureTogglesService<FeatureToggle> featureTogglesService)
        {
            _featureTogglesService = featureTogglesService;
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Count > 0)
            {
                options.EnsureNoAndOptions();
                options.EnsureNoOrOptions();
                
                var feature = options.Single();
                var featureToggle = _featureTogglesService.GetFeatureToggle(feature);

                if (!featureToggle.IsEnabled)
                {
                    authorizationResult.AddError(new FeatureNotEnabled());
                }
            }
            
            return Task.FromResult(authorizationResult);
        }
    }
}