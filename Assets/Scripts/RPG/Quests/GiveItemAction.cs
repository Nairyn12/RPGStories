using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Action_GiveItem_", menuName = "RPG/Dialogue/Action - Give Item")]
    public class GiveItemAction : DialogueAction
    {
        public ItemDataSO ItemToGive;
        public int Amount = 1;

        public override void Execute(GameObject player)
        {
            var inv = player.GetComponent<PlayerInventory>();
            if (inv != null && ItemToGive != null) inv.AddItem(ItemToGive, Amount);
        }
    }
}
