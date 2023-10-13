using System;

namespace Enjine.Data.InventorySystem
{
    public class TestItem : IInventoryItem
    {
        public bool IsEquipped { get; set; }
        public Type Type => GetType();
        public IInventoryItemInfo Info { get; }
        public IInventoryItemState State { get; }

        public TestItem(IInventoryItemInfo info)
        {
            this.Info = info;
            State = new InventoryItemState();
        }

        public IInventoryItem Clone()
        {
            var clonedItem = new TestItem(Info);
            clonedItem.State.Amount = State.Amount;
            return clonedItem;
        }
    }
}
