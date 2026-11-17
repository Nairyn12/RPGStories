using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Cond_HasQuest_", menuName = "RPG/Dialogue/Condition - Has Quest Active")]
    public class HasQuestCondition : DialogueCondition
    {
        public QuestDataSO QuestToCheck;

        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
        {
            if (QuestManager.Instance == null || QuestToCheck == null) return false;
            return QuestManager.Instance.IsQuestActive(QuestToCheck);
        }
    }
}
