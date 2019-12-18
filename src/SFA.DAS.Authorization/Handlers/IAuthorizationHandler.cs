namespace SFA.DAS.Authorization.Handlers
{
    public interface IAuthorizationHandler : IDefaultAuthorizationHandler
    {
        string Prefix { get; }
    }
}