using System.Linq;
using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Cond_Item_", menuName = "RPG/Dialogue/Condition - Item")]
    public class ItemCondition : DialogueCondition
    {
        public ItemDataSO RequiredItem;
        public bool MustBeEquipped = false;

        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
        {
            if (inventory == null || RequiredItem == null) return false;
            var slot = inventory.Slots.FirstOrDefault(s => s.Item == RequiredItem);
            if (slot == null) return false;
            if (MustBeEquipped && slot.EquippedSlot == EquipmentType.None) return false;
            return true;
        }
    }
}
