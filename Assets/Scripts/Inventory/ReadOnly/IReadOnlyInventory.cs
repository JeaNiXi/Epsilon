using System;
using UnityEngine;

namespace Inventory
{
    public interface IReadOnlyInventory
    {
        event Action<string, int> ItemsAdded;
        event Action<string, int> ItemsRemoved;

        string OwnerID {  get; }

        int GetAmount(string itemID);
        bool HasItems(string itemID, int amount);

    }
}