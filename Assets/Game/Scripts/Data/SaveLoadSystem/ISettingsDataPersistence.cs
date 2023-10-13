namespace Enjine.Data.SaveLoadSystem
{

    public interface ISettingsDataPersistence
    {
        void LoadData(SettingsData data);
        void SaveData(SettingsData data);
    }

}