using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Results;
using SFA.DAS.Authorization.Services;

namespace SFA.DAS.Authorization.Handlers
{

    //public class DefaultAuthorizationHandler : IDefaultAuthorizationHandler
    //{
    //    //private readonly IAuthorizationService _authorizationService;
    //    //public DefaultAuthorizationHandler(IAuthorizationService authorizationService )
    //    //{
    //    //    _authorizationService = authorizationService;
    //    //}
        
    //    public  Task<AuthorizationResult> GetAuthorizationResultDefault(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
    //    {
    //        return Task.FromResult(new AuthorizationResult());
    //    }
    //}

    public interface IDefaultAuthorizationHandler
    {
        Task<AuthorizationResult> GetAuthorizationResultDefault(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext);
    }

    public interface IAuthorizationHandler 
    {
        string Prefix { get; }

        Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext);
    }
}