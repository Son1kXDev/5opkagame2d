using UnityEngine;

namespace Enjine.Data
{
    public class ApplicationData : MonoBehaviour, IDataPersistence
    {
        public string NickName { get; private set; }
        public int CurrentProgress { get; private set; }


        public void LoadData(object data)
        {
            GameData game = (GameData)data;
            this.NickName = game.NickName;
            this.CurrentProgress = game.CurrentProgress;
        }

        public void SaveData(object data)
        {
            GameData game = (GameData)data;
            game.NickName = this.NickName;
            game.CurrentProgress = this.CurrentProgress;

            data = game;
        }
    }
}