using System;
using System.IO;
using MusicApp.Services;
using MusicApp.Services.Impl;

namespace MusicApp.Droid.Services
{
    public class DroidFileService : FileService
    {
        public DroidFileService()
        {
        }

        public override string GetStorageFolderPath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "Music");

            if (!Directory.Exists(libFolder))
                Directory.CreateDirectory(libFolder);

            return libFolder;
        }
    }
}
