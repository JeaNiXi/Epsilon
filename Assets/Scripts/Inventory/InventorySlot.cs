using Inventory;
using System;
using UnityEngine;

namespace Inventory
{
    public class InventorySlot : IReadOnlyInventorySlot
    {
        public event Action<string> ItemIDChanged;
        public event Action<int> ItemAmountChanged;

        public string ItemID
        {
            get => _data.ItemID;
            set
            {
                if (_data.ItemID != value)
                {
                    _data.ItemID = value;
                    ItemIDChanged?.Invoke(value);
                }
            }
        }
        public int Amount
        {
            get => _data.Amount;
            set
            {
                if (_data.Amount != value)
                {
                    _data.Amount = value;
                    ItemAmountChanged?.Invoke(value);
                }
            }
        }

        public bool IsEmpty => Amount == 0 && string.IsNullOrEmpty(ItemID);

        private readonly InventorySlotData _data;
        public InventorySlot(InventorySlotData slotData)
        {
            _data = slotData;
        }
    }
}