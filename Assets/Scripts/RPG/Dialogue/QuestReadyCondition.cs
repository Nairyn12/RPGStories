using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Cond_QuestReady_", menuName = "RPG/Dialogue/Condition - Quest Ready")]
    public class QuestReadyCondition : DialogueCondition
    {
        public QuestDataSO QuestToCheck;

        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
        {
            if (QuestManager.Instance == null || QuestToCheck == null) return false;
            return QuestManager.Instance.IsQuestReadyToTurnIn(QuestToCheck);
        }
    }
}