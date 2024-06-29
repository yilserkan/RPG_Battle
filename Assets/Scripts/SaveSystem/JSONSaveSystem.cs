using System.IO;
using UnityEngine;

namespace RPGGame.SaveSystem
{
    public class JSONSaveSystem<T> : ISaveSystem<T>
    {
        private readonly string _persistentDataPath;
        private readonly string _fileName;
        private readonly string _fileExtension;
        private ISerializer<T> _serializer;

        public JSONSaveSystem(string fileName)
        {
            _persistentDataPath = Application.persistentDataPath;
            _fileName = fileName;
            _fileExtension = "json";
            _serializer = new JSONSerializer<T>();
        }

        private string GetDataPath()
        {
            return Path.Combine(_persistentDataPath, string.Concat(_fileName, '.', _fileExtension));
        }

        public void Save(T data)
        {
            File.WriteAllText(GetDataPath(), _serializer.Serialize(data));
        }

        public void Load(out T obj)
        {
            var json = File.ReadAllText(GetDataPath());
            _serializer.Deserialize(json,out obj);
        }

        public void Delete()
        {
            var dataPath = GetDataPath();

            if (File.Exists(dataPath))
                File.Delete(dataPath);
        }

        public bool HasSaveFile()
        {
            return File.Exists(GetDataPath());
        }
    }
}