using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization
{
    public interface IAuthorizationHandler
    {
        Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> options, IAuthorizationContext authorizationContext);
    }
}