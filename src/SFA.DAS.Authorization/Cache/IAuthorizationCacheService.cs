using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.Cache
{
    public interface IAuthorizationCacheService
    {
        Task<AuthorizationResult> GetOrAdd(IAuthorizationHandler handler, IReadOnlyCollection<string> options,
            IAuthorizationContext authorizationContext);
    }
}