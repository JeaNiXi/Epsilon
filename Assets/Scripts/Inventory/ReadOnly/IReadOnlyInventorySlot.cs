using System;
using UnityEngine;

namespace Inventory
{

    public interface IReadOnlyInventorySlot
    {
        event Action<string> ItemIDChanged;
        event Action<int> ItemAmountChanged;

        string ItemID { get; }
        int Amount {  get; }
        bool IsEmpty { get; }
    }
}