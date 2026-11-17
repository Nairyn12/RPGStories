using UnityEngine;

namespace Game.RPG
{
    // Добавили IsNotTaken (Не взят)
    public enum QuestStateToCheck { IsNotTaken, IsActive, IsReadyToTurnIn, IsCompleted }

    [CreateAssetMenu(fileName = "Cond_QuestState_", menuName = "RPG/Dialogue/Condition - Quest State")]
    public class QuestStateCondition : DialogueCondition
    {
        public QuestDataSO QuestToCheck;
        public QuestStateToCheck StateToCheck;

        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
        {
            if (QuestManager.Instance == null || QuestToCheck == null) return false;

            switch (StateToCheck)
            {
                case QuestStateToCheck.IsNotTaken:
                    // Возвращает true, если квест НЕ взят и НЕ выполнен
                    return !QuestManager.Instance.IsQuestActive(QuestToCheck)
                        && !QuestManager.Instance.IsQuestCompleted(QuestToCheck);

                case QuestStateToCheck.IsActive:
                    return QuestManager.Instance.IsQuestActive(QuestToCheck);

                case QuestStateToCheck.IsReadyToTurnIn:
                    return QuestManager.Instance.IsQuestReadyToTurnIn(QuestToCheck);

                case QuestStateToCheck.IsCompleted:
                    return QuestManager.Instance.IsQuestCompleted(QuestToCheck);
            }

            return false;
        }
    }
}