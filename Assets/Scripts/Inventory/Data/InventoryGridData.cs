using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [Serializable]

    public class InventoryGridData
    {
        public string OwnerID;
        public List<InventorySlotData> SlotData;
        public Vector2Int Size;
    }
}