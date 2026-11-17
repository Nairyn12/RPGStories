using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Action_StartQuest_", menuName = "RPG/Dialogue/Action - Start Quest")]
    public class StartQuestAction : DialogueAction
    {
        public QuestDataSO QuestToStart;

        public override void Execute(GameObject player)
        {
            Debug.Log("[QuestAction] Метод Execute вызван!");

            if (QuestManager.Instance == null)
            {
                Debug.LogError("[QuestAction] ОШИБКА: QuestManager.Instance равен null!");
                return;
            }

            if (QuestToStart == null)
            {
                Debug.LogError("[QuestAction] ОШИБКА: В ассете не выбран квест (Quest To Start = None)!");
                return;
            }

            Debug.Log($"[QuestAction] Передаем квест '{QuestToStart.Title}' в QuestManager...");
            QuestManager.Instance.StartQuest(QuestToStart);
        }
    }
}