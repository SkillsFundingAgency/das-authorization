namespace SFA.DAS.Authorization
{
    public class DefaultAuthorizationContextProvider : IAuthorizationContextProvider
    {
        public IAuthorizationContext GetAuthorizationContext()
        {
            return new AuthorizationContext();
        }
    }
}