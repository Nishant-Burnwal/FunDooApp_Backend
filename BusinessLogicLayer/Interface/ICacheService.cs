using System;

namespace BusinessLogicLayer.Interface
{
    public interface ICacheService
    {
        void Set<T>(string key, T value, TimeSpan expiry);
        T Get<T>(string key);
        void Remove(string key);
    }
}
