using System.IO;
using UnityEngine;

namespace Klukva.RedKit.Persistence.Storage
{
    public class FileStorage : IStorage
    {
        private readonly string _basePath;
        private readonly string _fileName;

        public FileStorage(string fileName)
        {
            _basePath = Application.persistentDataPath;
            _fileName = fileName;
        }

        public bool Exists() => File.Exists(GetPath());

        public string Read() => File.ReadAllText(GetPath());

        public void Write(string data) => File.WriteAllText(GetPath(), data);

        public void Delete()
        {
            var path = GetPath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void Backup()
        {
            var path = GetPath();
            var backupPath = path + ".bak";
            if (File.Exists(path))
            {
                File.Copy(path, backupPath, overwrite: true);
            }
        }

        private string GetPath()
        {
            return Path.Combine(_basePath, _fileName + ".dat");
        }
    }
}