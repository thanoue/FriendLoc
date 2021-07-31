using System;
using System.IO;

namespace MusicApp.Services.Impl
{
    public abstract class FileService : IFileService
    {
        public FileService()
        {
        }

        public abstract string GetStorageFolderPath();


        public Stream OpenStream(string path, int bufferSize)
        {
            return new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, bufferSize);
        }

        public void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}