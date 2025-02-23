using Inventory;
using System;
using UnityEngine;

namespace Inventory
{
    public interface IReadOnlyInventoryGrid : IReadOnlyInventory
    {
        event Action<Vector2Int> SizeChanged;
        Vector2Int Size { get; }
        IReadOnlyInventorySlot[,] GetSlots();
    }
}