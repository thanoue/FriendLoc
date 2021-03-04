using System;
using System.IO;
using FriendLoc.Common;
using FriendLoc.Common.Services.Impl;

namespace FriendLoc.Droid.Services
{
    public class DroidFileService : MobileFileService
    {
        public DroidFileService()
        {

        }

        public DroidFileService(string rootFilePath)
        {
            SetRootFolderPath(rootFilePath);
        }

        public override string GetSdCardFolder()
        {
            var path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Constants.APP_NAME);

            if (!FolderExists(path))
            {
                CreateFolder(path);
            }

            return path;
        }

        public override string GetPersonalFolder()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}
