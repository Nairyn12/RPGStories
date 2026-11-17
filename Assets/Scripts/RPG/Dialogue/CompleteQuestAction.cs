using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Action_CompleteQuest_", menuName = "RPG/Dialogue/Action - Complete Quest")]
    public class CompleteQuestAction : DialogueAction
    {
        public QuestDataSO QuestToComplete;

        public override void Execute(GameObject player)
        {
            if (QuestManager.Instance != null && QuestToComplete != null)
            {
                QuestManager.Instance.TurnInQuest(QuestToComplete);
            }
        }
    }
}