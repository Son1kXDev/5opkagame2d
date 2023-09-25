using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enjine.Data
{
    public class ApplicationData : MonoBehaviour, Data.IDataPersistence
    {
        public string NickName { get; private set; }
        public int CurrentProgress { get; private set; }


        public void LoadData(GameData data)
        {
            this.NickName = data.NickName;
            this.CurrentProgress = data.CurrentProgress;
        }

        public void SaveData(GameData data)
        {
            data.NickName = this.NickName;
            data.CurrentProgress = this.CurrentProgress;
        }
    }
}