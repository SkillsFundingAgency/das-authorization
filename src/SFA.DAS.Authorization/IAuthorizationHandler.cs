﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization
{
    public interface IAuthorizationHandler
    {
        string Namespace { get; }

        Task<AuthorizationResult> GetAuthorizationResultAsync(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext);
    }
}