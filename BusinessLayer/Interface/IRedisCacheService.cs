using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IRedisCacheService
    {
        void SetCache<T>(string key, T value, TimeSpan expiration);
        T GetCache<T>(string key);
        void RemoveCache(string key);
    }
}
