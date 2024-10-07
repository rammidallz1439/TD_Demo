using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Vault
{
    public class DataManager : IController
    {
        private static DataManager instance;
        private readonly string saveDirectory = Application.persistentDataPath + "/Saves/";


        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataManager();
                }
                return instance;
            }
        }
        public void Save<T>(T data, string fileName)
        {
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            string filePath = saveDirectory + fileName;
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(fileStream, data);
            }
        }

        public T Load<T>(string fileName)
        {
            string filePath = saveDirectory + fileName;

            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    return (T)formatter.Deserialize(fileStream);
                }
            }
            else
            {
                
                Debug.LogWarning("Save file not found CReating path: " + filePath);
                return default;
            }
        }

        public bool SaveExists(string fileName)
        {
            return File.Exists(saveDirectory + fileName);
        }

        public void DeleteSave(string fileName)
        {
            string filePath = saveDirectory + fileName;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                Debug.LogWarning("Save file not found: " + filePath);
            }
        }


        public void SaveJson<T>(T data, string fileName)
        {
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            string filePath = Path.Combine(saveDirectory, fileName);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            File.WriteAllText(filePath, json);
            Debug.Log("Data Saved as JSON: " + json);
        }

        public T LoadJson<T>(string fileName)
        {
            string filePath = Path.Combine(saveDirectory, fileName);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                Debug.LogWarning("Save file not found: " + filePath);
                return default;
            }
        }

        #region LocalJson
     

        #endregion
        #region Contract methods
        public void OnInitialized()
        {
        }

        public void OnVisible()
        {
        }

        public void OnStarted()
        {
        }

        public void OnRegisterListeners()
        {
        }

        public void OnRemoveListeners()
        {
        }

        public void OnRelease()
        {
        }
        #endregion
    }
}

