
using UnityEngine;

namespace Fungus
{
    [CommandInfo("Item",
        "Change Item",
        "Adds or Removes an Item from the Inventory")]
    [AddComponentMenu("")]
    public class ChangeItem : Command
    {
        [Tooltip("Reference to an InventoryItem scriptable object that fills the ItemSlots in the Inventory")]
        [SerializeField] protected InventoryItem item;

        [Tooltip(
            "If add is true, item will be added to Inventory. If add is false, item will be removed from Inventory")]
        [SerializeField]
        protected bool add;

        public override void OnEnter()
        {
            if (item != null)
            {
                if (add)
                {
                    item.itemOwned = true;
                }
                else
                {
                    item.itemOwned = false;
                }
            }
            
            Continue();
        }

        public override string GetSummary()
        {
            if (item == null)
            {
                return "Error; No item selected";
            }
            
            return item.itemName;
        }
    }
}
