using UnityEngine;

namespace Inventory
{
    public class InventoryGridView : MonoBehaviour
    {
        private IReadOnlyInventoryGrid _inventory;
        public void Setup(IReadOnlyInventoryGrid inventory)
        {
            _inventory = inventory;
            Print();
        }

        public void Print()
        {
            var slots = _inventory.GetSlots();
            var size = _inventory.Size;
            var result = "";
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var slot = slots[i, j];
                    result += $"Slot ({i}:{j}. Item: {slot.ItemID}, amount: {slot.Amount}\n";
                }
            }
            Debug.Log(result);
        }
    }
}