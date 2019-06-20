namespace SFA.DAS.Authorization.Context
{
    public interface IAuthorizationContextProvider
    {
        IAuthorizationContext GetAuthorizationContext();
    }
}