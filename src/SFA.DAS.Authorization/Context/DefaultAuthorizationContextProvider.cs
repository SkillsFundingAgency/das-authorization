namespace SFA.DAS.Authorization.Context
{
    public class DefaultAuthorizationContextProvider : IAuthorizationContextProvider
    {
        public IAuthorizationContext GetAuthorizationContext()
        {
            return new AuthorizationContext();
        }
    }
}