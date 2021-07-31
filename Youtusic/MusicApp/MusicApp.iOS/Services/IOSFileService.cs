using System;
using MusicApp.Services.Impl;

namespace MusicApp.iOS.Services
{
    public class IOSFileService : FileService
    {
        public IOSFileService()
        {
        }

        public override string GetStorageFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
    }
}
