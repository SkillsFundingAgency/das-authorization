using System;
using SFA.DAS.Authorization.Context;

namespace SFA.DAS.Authorization.Caching
{
    public class AuthorizationContextCache : IAuthorizationContextProvider
    {
        private readonly Lazy<IAuthorizationContext> _authorizationContext;

        public AuthorizationContextCache(IAuthorizationContextProvider authorizationContextProvider)
        {
            _authorizationContext = new Lazy<IAuthorizationContext>(authorizationContextProvider.GetAuthorizationContext);
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            return _authorizationContext.Value;
        }
    }
}