﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.Handlers
{

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