using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphs;
using UnityEngine;

namespace Enjine.Data.InventorySystem
{
    public class Inventory : IInventory
    {
        public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;
        public event Action<object, Type, int> OnInventoryItemRemovedEvent;
        public event Action<object> OnInventoryStateChangedEvent;

        public int Capacity { get; set; }
        public bool IsFull => _slots.All(slot => slot.IsFull);
        private List<IInventorySlot> _slots;

        public Inventory(int capacity)
        {
            this.Capacity = capacity;

            _slots = new List<IInventorySlot>(capacity);

            for (int i = 0; i < capacity; i++)
                _slots.Add(new InventorySlot());
        }

        public IInventoryItem[] GetAllItems()
        {
            var allItems = new List<IInventoryItem>();
            foreach (var slot in _slots)
                if (slot.IsEmpty == false)
                    allItems.Add(slot.Item);

            return allItems.ToArray();
        }

        public IInventoryItem[] GetAllItems(Type itemType)
        {
            var allItemsOfType = new List<IInventoryItem>();
            var slotsOfType = _slots.
            FindAll(slot => slot.IsEmpty == false && slot.ItemType == itemType);

            foreach (var slot in slotsOfType)
                allItemsOfType.Add(slot.Item);

            return allItemsOfType.ToArray();
        }

        public IInventoryItem[] GetEquippedItems()
        {
            var equippedItems = new List<IInventoryItem>();
            var slotsWithEquippedItems = _slots.
            FindAll(slot => slot.IsEmpty == false && slot.Item.State.IsEquipped);

            foreach (var slot in slotsWithEquippedItems)
                equippedItems.Add(slot.Item);

            return equippedItems.ToArray();
        }

        public IInventoryItem GetItem(Type itemType)
        {
            return _slots.Find(slot => slot.ItemType == itemType).Item;
        }

        public int GetItemAmount(Type itemType)
        {
            var amount = 0;
            var slotsOfType = _slots.
            FindAll(slot => slot.IsEmpty == false && slot.ItemType == itemType);

            foreach (var slot in slotsOfType)
                amount += slot.Amount;

            return amount;
        }

        public bool TryAdd(object sender, IInventoryItem item)
        {
            var slotWIthSameItemButNotEmpty = _slots.Find(slot =>
                slot.IsEmpty == false &&
                slot.IsFull == false &&
                slot.ItemType == item.Type);

            if (slotWIthSameItemButNotEmpty != null)
                return TryAddToSlot(sender, slotWIthSameItemButNotEmpty, item);

            var emptySlot = _slots.Find(slot => slot.IsEmpty);
            if (emptySlot != null)
                return TryAddToSlot(sender, emptySlot, item);

            return false;
        }

        private bool TryAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
        {
            var fits = slot.Amount + item.State.Amount <= item.Info.StackSize;
            var amountToAdd = fits ? item.State.Amount : item.Info.StackSize - item.State.Amount;
            var amountLeft = item.State.Amount - amountToAdd;

            var clonedItem = item.Clone();
            clonedItem.State.Amount = amountToAdd;

            if (slot.IsEmpty) slot.SetItem(clonedItem);
            else slot.Item.State.Amount += amountToAdd;

            OnInventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);
            OnInventoryStateChangedEvent?.Invoke(sender);

            if (amountLeft <= 0)
                return true;

            item.State.Amount = amountLeft;

            return TryAdd(sender, item);
        }

        public void Remove(object sender, Type itemType, int amount = 1)
        {
            var slotsWithItem = GetAllSlots(itemType);
            if (slotsWithItem.Length == 0)
                return;

            var amountToRemove = amount;
            var count = slotsWithItem.Length;

            for (int i = count - 1; i >= 0; i--)
            {
                var slot = slotsWithItem[i];
                if (slot.Amount >= amountToRemove)
                {
                    slot.Item.State.Amount -= amountToRemove;

                    if (slot.Amount <= 0)
                        slot.Clear();

                    OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountToRemove);
                    OnInventoryStateChangedEvent?.Invoke(sender);

                    break;
                }

                var slotAmount = slot.Amount;
                amountToRemove -= slot.Amount;
                slot.Clear();

                OnInventoryItemRemovedEvent?.Invoke(sender, itemType, slotAmount);
                OnInventoryStateChangedEvent?.Invoke(sender);
            }
        }

        public void TransitFromSlotToSlot(object sender, IInventorySlot fromSlot, IInventorySlot toSlot)
        {
            if (fromSlot.IsEmpty) return;
            if (toSlot.IsFull) return;
            if (toSlot.IsEmpty == false && fromSlot.ItemType != toSlot.ItemType) return;
            if (fromSlot == toSlot) return;

            var slotCapacity = fromSlot.Capacity;
            var fits = fromSlot.Amount + toSlot.Amount <= slotCapacity;
            var amountToAdd = fits ? fromSlot.Amount : slotCapacity - toSlot.Amount;
            var amountLeft = fromSlot.Amount - amountToAdd;

            if (toSlot.IsEmpty)
            {
                toSlot.SetItem(fromSlot.Item);
                fromSlot.Clear();
                OnInventoryStateChangedEvent?.Invoke(sender);
                return;
            }

            toSlot.Item.State.Amount += amountToAdd;
            if (fits) fromSlot.Clear();
            else fromSlot.Item.State.Amount = amountLeft;

            OnInventoryStateChangedEvent?.Invoke(sender);
        }

        public IInventorySlot[] GetAllSlots()
        {
            return _slots.ToArray();
        }

        public IInventorySlot[] GetAllSlots(Type itemType)
        {
            return _slots
            .FindAll(slot => slot.IsEmpty == false && slot.ItemType == itemType)
            .ToArray();
        }


        public bool HasItem(Type itemType, out IInventoryItem item)
        {
            item = GetItem(itemType);
            return item != null;
        }

    }
}
