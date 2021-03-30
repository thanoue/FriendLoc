using System;
using Newtonsoft.Json;

namespace FriendLoc.Common.Services.Impl
{
    public abstract class SecureStorageService : ISecureStorageService
    {
        public abstract Boolean Store(string identifier, string content);
        public abstract String Fetch(string identifier);
        public abstract Boolean FetchBool(string identifier);
        public abstract Boolean Exist(string identifier);
        public abstract Boolean Remove(string identifier);

        public void StoreObject(string identifier, Object o)
        {
            var jsonStr = JsonConvert.SerializeObject(o);
            this.Store(identifier, jsonStr);
        }

        public T GetObject<T>(string identifier)
        {
            T obj = default(T);
            if (Exist(identifier))
            {
                var jsonStr = this.Fetch(identifier);
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    obj = JsonConvert.DeserializeObject<T>(jsonStr);
                }
            }

            return obj;
        }

        public void DeleteObject(string identifier)
        {
            if (Exist(identifier))
            {
                Remove(identifier);
            }
        }

    }
}
