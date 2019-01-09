using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerFeature.Namespace;
        
        private readonly IFeatureTogglesService _featureTogglesService;
        private readonly ILogger _logger;

        public AuthorizationHandler(IFeatureTogglesService featureTogglesService, ILogger logger)
        {
            _featureTogglesService = featureTogglesService;
            _logger = logger;
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
            {
                options.EnsureNoAndOptions();
                options.EnsureNoOrOptions();

                var values = authorizationContext.GetEmployerFeatureValues();
                var feature = options.Select(o => o.ToEnum<Feature>()).Single();
                var featureToggle = _featureTogglesService.GetFeatureToggle(feature);

                if (!featureToggle.IsEnabled)
                {
                    authorizationResult.AddError(new EmployerFeatureNotEnabled());
                }
                else if (featureToggle.IsWhitelistEnabled && !featureToggle.IsUserWhitelisted(values.AccountId, values.UserEmail))
                {
                    authorizationResult.AddError(new EmployerFeatureUserNotWhitelisted());
                }
            }
            
            var logResult = authorizationResult.Errors.Any() ? $"results '{authorizationResult.Errors.Select(o => o.GetType()).ToCsvString()}'" : "successful result";
            _logger.LogInformation($"Finished running '{this.GetType().FullName}' for options '{options.ToCsvString()}' with {logResult}");

            return Task.FromResult(authorizationResult);
        }
    }
}