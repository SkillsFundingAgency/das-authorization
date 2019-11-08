using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.Handlers
{
    public interface IDefaultAuthorizationHandler
    {
        Task<AuthorizationResult> GetDefaultAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext);
    }  
}
