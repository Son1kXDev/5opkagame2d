using System.Collections.Generic;

namespace Enjine.Data
{
    [System.Serializable]
    public class GameData
    {
        public string NickName;
        public int CurrentProgress;

        public GameData()
        {
            this.NickName = "Пятёрка";
            this.CurrentProgress = 0;
        }
    }
}