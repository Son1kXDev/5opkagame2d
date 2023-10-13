using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Enjine.Data.SaveLoadSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set; }

        public static bool Loaded;

        [Header("Debug")]
        [SerializeField] private bool _initializeDataIfNull = false;

        [Header("Storage Config")]
        [SerializeField, StatusIcon("")] private string _gameFileName;


        private GameData _gameData;
        private List<IDataPersistence> _dataPersistenceObjects;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            Loaded = false;
            if (transform.parent != null) transform.parent = null;
            DontDestroyOnLoad(this.gameObject);
            storage = new Storage(Application.persistentDataPath + "/saves", _gameFileName);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this._dataPersistenceObjects = FindAllDataPersistanceObjects();
            LoadGame();
        }

        private Storage storage;

        public void NewGame()
        { _gameData = new(); }

        [ContextMenu("Load games")]
        public void LoadGame()
        {
            _gameData = (GameData)storage.Load(new GameData());
            if (_gameData == null) return;
            foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
                dataPersistence.LoadData(_gameData);

            Debug.Log($"Game data is loaded");
        }

        [ContextMenu("Save game")]
        public void SaveGame()
        {
            if (_gameData == null) return;

            foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
                dataPersistence.SaveData(_gameData);

            storage.Save(_gameData);
        }


        [ContextMenu("Initialize Application Quit")]
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistanceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}