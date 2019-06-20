using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.Logging
{
    public static class LoggerExtensions
    {
        public static void LogAuthorizationResult<T>(this ILogger logger, T authorizationHandler, IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext, AuthorizationResult authorizationResult) where T : IAuthorizationHandler
        {
            logger.LogInformation($"Finished running '{typeof(T).FullName}' for options '{string.Join(", ", options)}' and context '{authorizationContext}' with result '{authorizationResult}'");
        }
    }
}