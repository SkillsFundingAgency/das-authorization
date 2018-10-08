namespace SFA.DAS.Authorization
{
    public interface IAuthorizationContext
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
    }
}