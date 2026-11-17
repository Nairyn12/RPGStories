using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Action_GiveXp_", menuName = "RPG/Dialogue/Action - Give XP")]
    public class GiveXpAction : DialogueAction
    {
        public int XpAmount = 50;

        public override void Execute(GameObject player)
        {
            var lvl = player.GetComponent<PlayerLeveling>();
            if (lvl != null) lvl.AddXp(XpAmount);
        }
    }
}
