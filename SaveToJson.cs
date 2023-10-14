  [Header("Save Config")] 
        [SerializeField] private string savePath;
        [SerializeField] private string saveFileName = "data.json";

       
        public void SaveToFile()
        {
            GameCoreStruct gameCore = new GameCoreStruct
            {
                characterId = this.characterId,
                selectedCharacter = this.selectedCharacter,
                characters = this.characters,
                
                lastLevelIndex = this.lastLevelIndex,
                levels = this.levels
            };

            string json = JsonUtility.ToJson(gameCore, true);

            try
            {
                File.WriteAllText(savePath, json);
            }
            catch (Exception e)
            {
                Debug.Log("{GameLog} => [GameCore] - (<color=red>Error</color>) - SaveToFile -> " + e.Message);                                                        
            }
        }

        public void LoadFromFile()
        {
            if (!File.Exists(savePath))
            {
                Debug.Log("{GameLog} => [GameCore] - LoadFromFile -> File Not Found!");
                return;
            }

            try
            {
                string json = File.ReadAllText(savePath);

                GameCoreStruct gameCoreFromJson = JsonUtility.FromJson<GameCoreStruct>(json);
                this.characterId = gameCoreFromJson.characterId;
                this.selectedCharacter = gameCoreFromJson.selectedCharacter;
                this.characters = gameCoreFromJson.characters;

                this.lastLevelIndex = gameCoreFromJson.lastLevelIndex;
                this.levels = gameCoreFromJson.levels;
            }
            catch (Exception e)
            {
                Debug.Log("{GameLog} - [GameCore] - (<color=red>Error</color>) - LoadFromFile -> " + e.Message);
            }
        }
        
        private void Awake()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            savePath = Path.Combine(Application.persistentDataPath, saveFileName);
#else
            savePath = Path.Combine(Application.dataPath, saveFileName);
#endif
            LoadFromFile();
        }

        private void OnApplicationQuit()
        {
            SaveToFile();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                SaveToFile();
            }
        }