using System.Collections.Generic;

namespace SFA.DAS.Authorization
{
    public class AuthorizationContext : IAuthorizationContext
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public T Get<T>(string key)
        {
            if (_data.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            throw new KeyNotFoundException($"The key '{key}' was not present in the dictionary");
        }

        public void Set<T>(string key, T value)
        {
            _data.Add(key, value);
        }
    }
}