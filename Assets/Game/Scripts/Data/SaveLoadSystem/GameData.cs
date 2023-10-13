using UnityEngine;

namespace Enjine.Data.SaveLoadSystem
{
    [System.Serializable]
    public class GameData
    {
        public string NickName;
        public int CurrentProgress;
        public Vector3 CurrentPlayerPosition;

        public SettingsData Settings;

        public GameData()
        {
            this.NickName = "Пятёрка";
            this.CurrentProgress = 0;
            this.CurrentPlayerPosition = Vector3.zero;
            this.Settings = new SettingsData();
        }
    }
}