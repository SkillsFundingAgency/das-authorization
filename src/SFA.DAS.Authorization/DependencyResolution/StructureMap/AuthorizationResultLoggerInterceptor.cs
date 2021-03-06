using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Logging;
using StructureMap.Building.Interception;

namespace SFA.DAS.Authorization.DependencyResolution.StructureMap
{
    public class AuthorizationResultLoggerInterceptor : FuncInterceptor<IAuthorizationHandler>
    {
        public AuthorizationResultLoggerInterceptor() : base((context, authorizationHandler) =>
            context.TryGetInstance<object>(nameof(AuthorizationRegistry)) == null
                ? new AuthorizationResultLogger(authorizationHandler, context.GetInstance<ILogger<AuthorizationResultLogger>>())
                : authorizationHandler) 
        {
            // TODO: Delete this class when SFA.DAS.Authorization.EmployerUserRoles has IServiceCollection extensions
        }
    }
}