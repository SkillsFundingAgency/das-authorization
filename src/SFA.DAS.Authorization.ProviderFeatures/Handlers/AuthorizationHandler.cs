using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Extensions;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Logging;
using SFA.DAS.Authorization.ProviderFeatures.Context;
using SFA.DAS.Authorization.ProviderFeatures.Errors;
using SFA.DAS.Authorization.ProviderFeatures.Models;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.ProviderFeatures.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => "ProviderFeature.";
        
        private readonly IFeatureTogglesService<ProviderFeatureToggle> _featureTogglesService;
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(IFeatureTogglesService<ProviderFeatureToggle> featureTogglesService, ILogger<AuthorizationHandler> logger)
        {
            _featureTogglesService = featureTogglesService;
            _logger = logger;
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
                    authorizationResult.AddError(new ProviderFeatureNotEnabled());
                }
                else if (featureToggle.IsWhitelistEnabled)
                {
                    var values = authorizationContext.GetProviderFeatureValues();

                    if (!featureToggle.IsUserWhitelisted(values.Ukprn, values.UserEmail))
                    {
                        authorizationResult.AddError(new ProviderFeatureUserNotWhitelisted());
                    }
                }
            }
            
            _logger.LogAuthorizationResult(this, options, authorizationContext, authorizationResult);
            
            return Task.FromResult(authorizationResult);
        }
    }
}