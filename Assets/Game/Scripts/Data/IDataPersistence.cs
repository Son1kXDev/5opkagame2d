using UnityEditor;
using UnityEngine;
namespace Enjine.Data
{

    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(GameData data);
    }

}