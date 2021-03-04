using System;
using System.Collections.Generic;
using System.IO;

namespace FriendLoc.Common.Services
{
    public interface IFileService 
    {
        Stream CreateFile(string path);

        bool FileExists(string path);

        Stream OpenFile(string path);

        byte[] OpenFileAsByte(string path);

        string OpenFileAsString(string path);

        void WriteToFile(string path, byte[] data);

        void WriteToFile(string path, string data);

        void DeleteFile(string path);

        bool FolderExists(string path);

        void CreateFolder(string path);

        void DeleteFolder(string path);

        string GetFilePath(string folder, string file);

        List<string> GetFiles(string path);

        List<string> GetFileNames(string folderpath);

        string GetGeneralFolder(string folder);

        void DeleteAllExcept(string filePath);

        void SetRootFolderPath(string path);

        string GetSdCardFolder();

        string GetPersonalFolder();
    }
}
