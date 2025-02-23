using Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Test
{
    public class InventoryGrid : IReadOnlyInventoryGrid
    {
        public event Action<Vector2Int> SizeChanged;
        public event Action<string, int> ItemsAdded;
        public event Action<string, int> ItemsRemoved;
        public string OwnerID => _data.OwnerID;

        public Vector2Int Size
        {
            get => _data.Size;
            set
            {
                if (_data.Size != value)
                {
                    _data.Size = value;
                    SizeChanged?.Invoke(value);
                }
            }
        }
        private readonly InventoryGridData _data;
        private readonly Dictionary<Vector2Int, InventorySlot> _slotsMap = new();
        public InventoryGrid(InventoryGridData gridData)
        {
            _data = gridData;

            var size = gridData.Size;
            for (int i = 0; i < gridData.Size.x; i++)
            {
                for (int j = 0; j < gridData.Size.y; j++)
                {
                    var index = i * size.y + j;
                    var slotData = gridData.SlotData[index];
                    var slot = new InventorySlot(slotData);
                    var position = new Vector2Int(i, j);

                    _slotsMap[position] = slot;
                }
            }
        }
        public AddItemsToInventoryGridResult AddItems(string itemID, int amount = 1)
        {
            var remainingAmount = amount;
            var itemsAddedToSlotsWithSameItemsAmount = AddToSlotWithSameItems(itemID, remainingAmount, out  remainingAmount);

            if (remainingAmount <= 0)
            {
                return new AddItemsToInventoryGridResult(OwnerID, amount, itemsAddedToSlotsWithSameItemsAmount);
            }

            var itemsAddedToAvailableSlotsAmount = AddToFirstAvailableSlots(itemID, remainingAmount, out remainingAmount);
            var totalAddedItemsAmount = itemsAddedToSlotsWithSameItemsAmount + itemsAddedToAvailableSlotsAmount;

            return new AddItemsToInventoryGridResult(OwnerID, amount, totalAddedItemsAmount);
        }
        public AddItemsToInventoryGridResult AddItems(Vector2Int slotCoords, string itemID, int amount = 1)
        {
            var slot = _slotsMap[slotCoords];
            var newValue = slot.Amount + amount;
            var itemsAddedAmount = 0;

            if (slot.IsEmpty)
            {
                slot.ItemID = itemID;
            }

            var itemSlotCapacity = GetItemSlotCapacity(itemID);

            if (newValue > itemSlotCapacity)
            {
                var remainingItems = newValue - itemSlotCapacity;
                var itemsToAddAmount = itemSlotCapacity - slot.Amount;
                itemsAddedAmount += itemsToAddAmount;
                slot.Amount = itemSlotCapacity;

                var result = AddItems(itemID, remainingItems);
                itemsAddedAmount += result.ItemsAddedAmount;
            }
            else
            {
                itemsAddedAmount = amount;
                slot.Amount = newValue;
            }
            return new AddItemsToInventoryGridResult(OwnerID, amount, itemsAddedAmount);
        }



        public RemoveItemsFromInventoryGridResult RemoveItems(string itemID, int amount = 1)
        {
            if (!HasItems(itemID, amount))
            {
                return new RemoveItemsFromInventoryGridResult(OwnerID, amount, false);
            }

            var amountToRemove = amount;
            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    var slotCoords = new Vector2Int(i, j);
                    var slot = _slotsMap[slotCoords];

                    if (slot.ItemID != itemID)
                    { continue; }

                    if (amountToRemove > slot.Amount) 
                    {
                        amountToRemove -= slot.Amount;
                        RemoveItems(slotCoords, itemID, slot.Amount);
                    }
                    else
                    {
                        RemoveItems(slotCoords, itemID, amountToRemove);
                        return new RemoveItemsFromInventoryGridResult(OwnerID, amount, true);
                    }
                }
            }
            throw new Exception("Something went wrong, couldn't remove some items");
        }
        public RemoveItemsFromInventoryGridResult RemoveItems(Vector2Int slotCoords, string itemID, int amount = 1)
        {
            var slot = _slotsMap[slotCoords];

            if (slot.IsEmpty || slot.ItemID != itemID || slot.Amount < amount)
            {
                return new RemoveItemsFromInventoryGridResult(OwnerID, amount, false);
            }
            slot.Amount -= amount;
            if (slot.Amount == 0)
            {
                slot.ItemID = null;
            }
            return new RemoveItemsFromInventoryGridResult(OwnerID, amount, true);
        }
        public int GetAmount(string itemID)
        {
            var amount = 0;
            var slots = _data.SlotData;

            foreach (var slot in slots)
            {
                if (slot.ItemID == itemID) 
                    amount += slot.Amount;
            }
            return amount;
        }

        public IReadOnlyInventorySlot[,] GetSlots()
        {
            var array = new IReadOnlyInventorySlot[Size.x, Size.y];

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    var position = new Vector2Int(i, j);
                    array[i,j] = _slotsMap[position];
                }
            }
            return array;
        }

        public bool HasItems(string itemID, int amount)
        {
            var amountExist = GetAmount(itemID);
            return amountExist >= amount;
        }
        private int GetItemSlotCapacity(string itemID)
        {
            return 99;
        }
        private int AddToSlotWithSameItems(string itemID, int amount, out int remainingAmount)
        {
            var itemsAddedAmount = 0;
            remainingAmount = amount;

            for(var i=0;i<Size.x;i++)
            {
                for(var j=0;j<Size.y;j++)
                {
                    var coords = new Vector2Int(i,j);
                    var slot = _slotsMap[coords];

                    if (slot.IsEmpty) 
                    { continue; }

                    var slotItemCapacity = GetItemSlotCapacity(slot.ItemID);
                    if (slot.Amount >= slotItemCapacity)
                    { continue; }

                    if (slot.ItemID != itemID) 
                    { continue; }

                    var newValue = slot.Amount + remainingAmount;
                    if (newValue > slotItemCapacity)
                    {
                        remainingAmount = newValue - slotItemCapacity;
                        var itemsToAddAmount = slotItemCapacity - slot.Amount;
                        itemsAddedAmount += itemsToAddAmount;
                        slot.Amount = slotItemCapacity;

                        if (remainingAmount == 0)
                            return itemsAddedAmount;
                    }
                    else
                    {
                        itemsAddedAmount += remainingAmount;
                        slot.Amount = newValue;
                        remainingAmount = 0;

                        return itemsAddedAmount;
                    }
                }
            }
            return itemsAddedAmount;
        }
        private int AddToFirstAvailableSlots(string itemID, int amount, out int remainingAmount)
        {
            var itemsAddedAmount = 0;
            remainingAmount = amount;

            for (var i = 0; i < Size.x; i++)
            {
                for (var j = 0; j < Size.y; j++)
                {
                    var coords = new Vector2Int(i, j);
                    var slot = _slotsMap[coords];

                    if (!slot.IsEmpty)
                    { continue; }

                    slot.ItemID = itemID;
                    var newValue = remainingAmount;
                    var slotItemCapacity = GetItemSlotCapacity(slot.ItemID);

                    if (newValue > slotItemCapacity)
                    {
                        remainingAmount = newValue - slotItemCapacity;
                        var itemsToAddAmount = slotItemCapacity;
                        itemsAddedAmount += itemsToAddAmount;
                        slot.Amount = slotItemCapacity;
                    }
                    else
                    {
                        itemsAddedAmount += remainingAmount;
                        slot.Amount = newValue;
                        remainingAmount = 0;

                        return itemsAddedAmount;
                    }
                }
            }
            return itemsAddedAmount;
        }

        public void SwitchSlots(Vector2Int slotCoordsA, Vector2Int slotCoordsB)
        {
            var slotA = _slotsMap[slotCoordsA];
            var slotB = _slotsMap[slotCoordsB];
            var tempSlotItemID = slotA.ItemID; 
            var tempSlotItemAmount = slotA.Amount;
            slotA.ItemID = slotB.ItemID;
            slotA.Amount = slotB.Amount;
            slotB.ItemID = tempSlotItemID;
            slotB.Amount = tempSlotItemAmount;
        }
        public void SetSize(Vector2Int newSize)
        {
            throw new NotImplementedException();
        }
    }
}