using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FriendLoc.Common.Services.Impl
{
    public abstract class MobileFileService : IFileService
    {
        public MobileFileService()
        {
        }

        public abstract string GetPersonalFolder();
        public abstract string GetSdCardFolder();

        private string _rootPath = "";

        public void DeleteAllExcept(string filePath)
        {
            var files = GetFiles(Directory.GetParent(filePath).FullName);

            if (files == null || !files.Any())
                return;

            foreach (var file in files)
            {
                if (Path.GetFileName(file).Equals(Path.GetFileName(filePath)))
                    continue;

                File.Delete(file);
            }
        }

        public void SetRootFolderPath(string path)
        {
            _rootPath = path;
        }

        public string GetGeneralFolder(string folderName)
        {
            if (string.IsNullOrEmpty(_rootPath))
                _rootPath = GetSdCardFolder();

            var path = Path.Combine(_rootPath, folderName);

            if (!FolderExists(path)) CreateFolder(path);

            return path;
        }

        public Stream CreateFile(string path)
        {
            return File.Create(path);
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteFile(string path)
        {
            if (FileExists(path))
                File.Delete(path);
        }

        public void DeleteFolder(string path)
        {
            if (FolderExists(path))
                Directory.Delete(path, true);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool FolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public string GetFilePath(string folder, string file)
        {
            if (folder != null && file != null)
            {
                return Path.Combine(folder, file);
            }
            else
            {
                return null;
            }
        }

        public List<string> GetFiles(string path)
        {
            List<string> res = new List<string>();

            if (!FolderExists(path))
            {
                return res;
            }

            return Directory.GetFiles(path).ToList();
        }

        public List<string> GetFileNames(string folderpath)
        {
            List<string> res = new List<string>();

            if (!FolderExists(folderpath))
            {
                return res;
            }

            var files = Directory.GetFiles(folderpath).ToList();

            if (files != null && files.Count != 0)
            {
                foreach (var item in files)
                {
                    string file = string.Empty;

                    try
                    {
                        file = item.Substring(item.LastIndexOf('/') + 1, item.LastIndexOf(".") - item.LastIndexOf('/') - 1);
                    }
                    catch (System.Exception)
                    {
                    }

                    if (!string.IsNullOrWhiteSpace(file))
                    {
                        res.Add(file);
                    }
                }
            }

            return res;
        }

        public Stream OpenFile(string path)
        {
            return File.OpenRead(path);
        }

        public byte[] OpenFileAsByte(string path)
        {
            return File.ReadAllBytes(path);
        }

        public string OpenFileAsString(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteToFile(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }

        public void WriteToFile(string path, string data)
        {
            File.WriteAllText(path, data);
        }
    }
}
