using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Cond_Stat_", menuName = "RPG/Dialogue/Condition - Stat")]
    public class StatCondition : DialogueCondition
    {
        public StatType Stat;
        public int RequiredValue;

        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
        {
            if (stats == null) return false;
            return stats.GetStat(Stat) >= RequiredValue;
        }
    }
}