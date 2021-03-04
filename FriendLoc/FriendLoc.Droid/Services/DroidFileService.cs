using System;
using System.IO;
using Android.OS;
using FriendLoc.Common;
using FriendLoc.Common.Services.Impl;
using Plugin.CurrentActivity;

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
            var path = "";

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                var activity = CrossCurrentActivity.Current.Activity;

                path = Path.Combine(activity.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments).AbsolutePath, Constants.APP_NAME);
            }
            else
            {
                path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Constants.APP_NAME);
            }

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
