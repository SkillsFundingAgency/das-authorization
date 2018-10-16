namespace SFA.DAS.Authorization
{
    public interface IAuthorizationContextProvider
    {
        IAuthorizationContext GetAuthorizationContext();
    }
}