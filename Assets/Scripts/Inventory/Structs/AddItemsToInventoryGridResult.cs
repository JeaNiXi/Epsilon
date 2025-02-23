using UnityEngine;

namespace Inventory
{
    public readonly struct AddItemsToInventoryGridResult
    {
        public readonly string InventoryOwnerID;
        public readonly int ItemsToAddAmount;
        public readonly int ItemsAddedAmount;

        public int ItemsNotAddedAmount => ItemsToAddAmount - ItemsAddedAmount;

        public AddItemsToInventoryGridResult(
            string inventoryOwnerID,
            int itemsToAddAmount,
            int itemsAddedAmount)
        {
            InventoryOwnerID = inventoryOwnerID;
            ItemsToAddAmount = itemsToAddAmount;
            ItemsAddedAmount = itemsAddedAmount;
        }
    }
}