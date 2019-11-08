using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.Handlers
{
    public class DefaultAuthorizationHandler : IDefaultAuthorizationHandler
    {
        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            return Task.FromResult(new AuthorizationResult());
        }      
    }
}
