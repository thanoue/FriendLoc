using System.IO;
using Android.App;
using Android.Content;
using Android.Preferences;
using MusicApp.Services;
using MusicApp.Services.Impl;
using MusicApp.Static;
using PCLStorage;

namespace MusicApp.Droid.Services
{
    public class DroidSecureStorageService : SecureStorageService
    {
        string passwordhash = "sdDD%7!32d";
        bool encrypted = false;
        ISharedPreferences prefs;
        ISharedPreferencesEditor editor;
        protected readonly IFolder _rootFolder;

        public DroidSecureStorageService()
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
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
        // public override bool FolderExists(string folderpath)
        // {
        //     return Directory.Exists(folderpath);
        // }
        //
        // public override void CreateFolder(string folderpath)
        // {
        //     if (!FolderExists(folderpath))
        //     {
        //         IFolder rootFolder = _rootFolder;
        //         rootFolder.CreateFolderAsync(folderpath, CreationCollisionOption.OpenIfExists).ConfigureAwait(false).GetAwaiter().GetResult();
        //     }
        // }
        // public override string GetExternalStoragePath()
        // {
        //     return global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        // }
        //
        // public override string DownloadFolder()
        // {
        //     var storePath = Fetch("download_folder_path");
        //     if (string.IsNullOrWhiteSpace(storePath))
        //     {
        //         storePath = GetExternalStoragePath() + MobileConstants.ROOTFOLDERPATH + "/download";
        //         if (!FolderExists(storePath))
        //             CreateFolder(storePath);
        //     }
        //     return storePath;
        // }
    }
}