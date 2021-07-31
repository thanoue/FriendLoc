using System;

namespace MusicApp.Services
{
    public interface ISecureStorageService
    {
        Boolean Store(string identifier, string content);
        String Fetch(string identifier);
        bool FetchBool(string identifier);
        Boolean Exist(string identifier);
        Boolean Remove(string identifier);

        void StoreObject(string identifier, Object o);
        T GetObject<T>(string identifier);
        void DeleteObject(string identifier);
    }
}