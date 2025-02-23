using System;
using System.Collections.Generic;
using Test;
using UnityEngine;

namespace Inventory
{
    public class InventoryService
    {
        private readonly Dictionary<string, InventoryGrid> _inventoriesMap = new();

        public InventoryGrid RegisterInventory(InventoryGridData inventoryData)
        {
            var inventory = new InventoryGrid(inventoryData);
            _inventoriesMap[inventory.OwnerID] = inventory;

            return inventory;
        }

        public AddItemsToInventoryGridResult AddItems(string ownerID, string itemID, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerID];
            return inventory.AddItems(itemID, amount);
        }
        public AddItemsToInventoryGridResult AddItems(string ownerID, Vector2Int slotCoords, string itemID, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerID];
            return inventory.AddItems(slotCoords, itemID, amount);
        }



        public RemoveItemsFromInventoryGridResult RemoveItems(string ownerID, string itemID, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerID];
            return inventory.RemoveItems(itemID, amount);
        }
        public RemoveItemsFromInventoryGridResult RemoveItems(string ownerID, Vector2Int slotCoords, string itemID, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerID];
            return inventory.RemoveItems(slotCoords, itemID, amount);
        }

        public IReadOnlyInventoryGrid GetInventory(string ownerID)
        {
            return _inventoriesMap[ownerID];
        }
    }
}