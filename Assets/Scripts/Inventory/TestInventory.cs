using Inventory;
using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Test
{
    public class TestInventory : MonoBehaviour
    {
        public InventoryGridView _view;
        private InventoryService _inventoryService;

        private void Start()
        {
            _inventoryService = new InventoryService();

            var ownerID = "testoSErv";
            var inventoryData = CreateTestInventory(ownerID);
            var inventory = _inventoryService.RegisterInventory(inventoryData);

            _view.Setup(inventory);

            var addedResult = _inventoryService.AddItems(ownerID, "apple", 30);
            Debug.Log($"Items added. ItemID: apple, amount to add: 30, amount added: {addedResult.ItemsAddedAmount}");
            addedResult = _inventoryService.AddItems(ownerID, "brick", 112);
            Debug.Log($"Items added. ItemID: brick, amount to add: 112, amount added: {addedResult.ItemsAddedAmount}");
            addedResult = _inventoryService.AddItems(ownerID, "latter", 10);
            Debug.Log($"Items added. ItemID: latter, amount to add: 10, amount added: {addedResult.ItemsAddedAmount}");

            _view.Print();

            var removeResult = _inventoryService.RemoveItems(ownerID, "apple", 13);
            Debug.Log($"Items removed. ItemID: apple, amount to add: 13, success {removeResult.Success}");

            _view.Print();

            removeResult = _inventoryService.RemoveItems(ownerID, "apple", 18);
            Debug.Log($"Items removed. ItemID: apple, amount to add: 18, success {removeResult.Success}");

            _view.Print();
            //    var inventoryData = new InventoryGridData
            //    {
            //        Size = new Vector2Int(3, 4),
            //        OwnerID = "Tester",
            //        SlotData = new List<InventorySlotData>()
            //    };

            //    var size = inventoryData.Size;
            //    for (int i = 0; i < size.x; i++)
            //    {
            //        for (int j = 0; j < size.y; j++)
            //        {
            //            var index = i * size.y + j;
            //            Debug.Log($"i = {i}, size.y = {size.y}, j = {j} i*size.y+j = index is: {index} .");
            //            inventoryData.SlotData.Add(new InventorySlotData());
            //        }
            //    }

            //    var slotData = inventoryData.SlotData[0];
            //    slotData.ItemID = "tester";
            //    slotData.Amount = 14;

            //    var inventory = new InventoryGrid(inventoryData);

            //    _view.Setup(inventory);
        }
        private InventoryGridData CreateTestInventory(string ownerID)
        {
            var size = new Vector2Int(3, 4);
            var createdInventorySlots = new List<InventorySlotData>();
            var length = size.x * size.y;
            for (var i = 0; i < length; i++)
            {
                createdInventorySlots.Add(new InventorySlotData());
            }

            var createdInventoryData = new InventoryGridData
            {
                OwnerID = ownerID,
                Size = size,
                SlotData = createdInventorySlots
            };
            return createdInventoryData;
        }
    }
}