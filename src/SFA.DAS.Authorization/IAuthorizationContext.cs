﻿namespace SFA.DAS.Authorization
{
    public interface IAuthorizationContext
    {
        T Get<T>(string key);
        void Add<T>(string key, T value);
        bool TryGet<T>(string key, out T value);
    }
}