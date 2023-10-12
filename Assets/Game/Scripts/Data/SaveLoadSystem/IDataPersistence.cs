
namespace Enjine.Data
{

    public interface IDataPersistence
    {
        void LoadData(object data);
        void SaveData(object data);
    }

}