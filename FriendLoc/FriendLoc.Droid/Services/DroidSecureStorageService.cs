using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using FriendLoc.Common.Services;
using FriendLoc.Common.Services.Impl;
using FriendLoc.Common.Statistics;

namespace FriendLoc.Droid.Services
{
    public class DroidSecureStorageService : SecureStorageService, ISecureStorageService
    {
        string passwordhash = "sdDD%7!32d";
        bool encrypted = false;
        ISharedPreferences prefs;
        ISharedPreferencesEditor editor;

        public DroidSecureStorageService()
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            editor = prefs.Edit();
        }

        public DroidSecureStorageService(Application context)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            editor = prefs.Edit();
        }

        public override bool Exist(string identifier)
        {
            if (encrypted)
            {
                var encryptedKey = Crypto.AESEncrypt(identifier, passwordhash);
                return !string.IsNullOrWhiteSpace(prefs.GetString(encryptedKey, null));
            }

            return prefs.Contains(identifier);
        }

        public override string Fetch(string identifier)
        {
            if (encrypted)
            {
                var encryptedKey = Crypto.AESEncrypt(identifier, passwordhash);
                var resultEncrypted = prefs.GetString(encryptedKey, null);
                if (string.IsNullOrWhiteSpace(resultEncrypted)) return null;
                return Crypto.AESDecrypt(resultEncrypted, passwordhash);
            }
            else
            {
                return prefs.GetString(identifier, null);
            }
        }

        public override bool Store(string identifier, string content)
        {
            if (encrypted)
            {
                var encryptedKey = Crypto.AESEncrypt(identifier, passwordhash);
                var encryptedContent = Crypto.AESDecrypt(content, passwordhash);
                editor.PutString(encryptedKey, encryptedContent);
                editor.Apply();
                return true;
            }
            else
            {
                editor.PutString(identifier, content);
                editor.Apply();
                return true;
            }
        }

        public override bool Remove(string identifier)
        {
            if (encrypted)
            {
                var encryptedKey = Crypto.AESEncrypt(identifier, passwordhash);
                editor.PutString(encryptedKey, null);
                editor.Apply();
                return true;
            }
            else
            {
                editor.PutString(identifier, null);
                editor.Apply();
                return true;
            }
        }

        public override bool FetchBool(string identifier)
        {
            if (encrypted)
            {
                var encryptedKey = Crypto.AESEncrypt(identifier, passwordhash);
                return prefs.GetBoolean(encryptedKey, false);
            }
            else
            {
                return prefs.GetBoolean(identifier, false);
            }
        }

    }
}
