using System;

namespace Enjine.Data.InventorySystem
{
    public interface IInventoryItem
    {
        IInventoryItemInfo Info { get; }
        IInventoryItemState State { get; }
        Type Type { get; }

        IInventoryItem Clone();
    }
}
