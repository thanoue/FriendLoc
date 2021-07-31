using System;
using System.IO;

namespace MusicApp.Services
{
    public interface IFileService
    {
        string GetStorageFolderPath();
        Stream OpenStream(string path, int bufferSize);

        void DeleteFile(string path);
    }
}
