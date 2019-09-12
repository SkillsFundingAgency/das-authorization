using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.EmployerFeatures.Context;
using SFA.DAS.Authorization.EmployerFeatures.Errors;
using SFA.DAS.Authorization.EmployerFeatures.Models;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Options;
using SFA.DAS.Authorization.Results;
using SFA.DAS.EmployerAccounts.Api.Client;

namespace SFA.DAS.Authorization.EmployerFeatures.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => "EmployerFeature.";

        private readonly IFeatureTogglesService<EmployerFeatureToggle> _featureTogglesService;
        private readonly IEmployerAccountsApiClient _employerAccountsApiClient;

        public AuthorizationHandler(IFeatureTogglesService<EmployerFeatureToggle> featureTogglesService, IEmployerAccountsApiClient employerAccountsApiClient)
        {
            _featureTogglesService = featureTogglesService;
            _employerAccountsApiClient = employerAccountsApiClient;
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
                    authorizationResult.AddError(new EmployerFeatureNotEnabled());
                }
                else if (featureToggle.IsWhitelistEnabled)
                {
                    var values = authorizationContext.GetEmployerFeatureValues();
                    
                    if (!featureToggle.IsUserWhitelisted(values.AccountId, values.UserEmail))
                    {
                        authorizationResult.AddError(new EmployerFeatureUserNotWhitelisted());
                    }
                }
                else if (
                    authorizationContext.TryGet<int>(AuthorizationContextKey.AgreementVersion, out var agreementVersion) &&
                    !_employerAccountsApiClient.HasAgreementBeenSigned(
                             new HasAgreementBeenSignedRequest {
                                 AccountId = authorizationContext.GetEmployerFeatureValues().AccountId,
                                 AgreementType = authorizationContext.Get<string>(AuthorizationContextKey.AgreementType),
                                 AgreementVersion = agreementVersion
                             }, CancellationToken.None).Result)
                {
                    authorizationResult.AddError(new EmployerFeatureAgreementNotSigned());
                }
            }
            
            return Task.FromResult(authorizationResult);
        }
    }
}