using System.Collections;
using System.Collections.Generic;

namespace SFA.DAS.Authorization
{
    public class AuthorizationContext : IAuthorizationContext, IEnumerable
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
        
        public T Get<T>(string key)
        {
            if (_data.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            throw new KeyNotFoundException($"The key '{key}' was not present in the authorization context");
        }

        public void Add<T>(string key, T value)
        {
            _data.Add(key, value);
        }

        public bool TryGet<T>(string key, out T value)
        {
            var exists = _data.TryGetValue(key, out var obj);

            value = exists ? (T)obj : default;

            return exists;
        }
        
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable) _data).GetEnumerator();
        }
    }
}