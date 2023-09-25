using UnityEditor;
using UnityEngine;

namespace Enjine.Data
{
    public interface ISettingsDataPersistence
    {
        void LoadData(SettingsData data);
        void SaveData(SettingsData data);
    }

}