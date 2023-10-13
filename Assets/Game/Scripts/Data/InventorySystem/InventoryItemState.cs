using System;

namespace Enjine.Data.InventorySystem
{
    [Serializable]
    public class InventoryItemState : IInventoryItemState
    {
        public int itemAmount;
        public bool isItemEquipped;

        public int Amount { get => itemAmount; set => itemAmount = value; }
        public bool IsEquipped { get => isItemEquipped; set => isItemEquipped = value; }

        public InventoryItemState()
        {
            itemAmount = 0;
            isItemEquipped = false;
        }
    }
}
