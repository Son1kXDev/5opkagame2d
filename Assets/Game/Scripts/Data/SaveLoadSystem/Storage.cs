using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

namespace Enjine.Data.SaveLoadSystem
{
    public class Storage
    {
        private string _filePath;
        private BinaryFormatter _formatter;

        public Storage(string fileDirectory, string fileName)
        {
            if (!Directory.Exists(fileDirectory)) Directory.CreateDirectory(fileDirectory);
            _filePath = fileDirectory + "/" + fileName;
            InitializeBinaryFormatter();
        }

        private void InitializeBinaryFormatter()
        {
            _formatter = new BinaryFormatter();

            var selector = new SurrogateSelector();

            var v3Surrogate = new Vector3SerializationSurrogate();

            selector.AddSurrogate(typeof(Vector3),
            new StreamingContext(StreamingContextStates.All), v3Surrogate);

            _formatter.SurrogateSelector = selector;

        }

        public object Load(object defaultData)
        {
            if (!File.Exists(_filePath))
            {
                if (defaultData != null)
                    Save(defaultData);
                return defaultData;
            }

            var file = File.Open(_filePath, FileMode.Open);
            var data = _formatter.Deserialize(file);
            file.Close();

            return data;
        }

        public void Save(object data)
        {
            var file = File.Create(_filePath);
            _formatter.Serialize(file, data);
            file.Close();
        }
    }
}
