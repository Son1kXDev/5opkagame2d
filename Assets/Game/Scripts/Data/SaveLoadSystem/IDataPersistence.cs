
namespace Enjine.Data.SaveLoadSystem
{

    public interface IDataPersistence
    {
        void LoadData(object data);
        void SaveData(object data);
    }

}