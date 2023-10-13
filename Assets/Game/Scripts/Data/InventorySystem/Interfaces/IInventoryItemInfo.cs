using UnityEngine;

namespace Enjine.Data.InventorySystem
{
    public interface IInventoryItemInfo
    {
        string ID { get; }
        string Title { get; }
        string Description { get; }
        int StackSize { get; }
        Sprite Icon { get; }

    }
}
