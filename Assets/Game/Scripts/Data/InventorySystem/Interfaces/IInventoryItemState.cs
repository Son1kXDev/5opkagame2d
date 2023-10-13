using System;

namespace Enjine.Data.InventorySystem
{
    public interface IInventoryItemState
    {
        int Amount { get; set; }
        bool IsEquipped { get; set; }
    }
}
