using System.Collections.Generic;
using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Dialogue_", menuName = "RPG/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        public string NpcName;
        public List<DialogueNode> Nodes = new();
    }

    [System.Serializable]
    public class DialogueNode
    {
        [TextArea(2, 5)]
        public string NpcText;

        [Tooltip("Условия для старта диалога с этого узла. Если пусто - подходит всем.")]
        public List<DialogueCondition> Conditions = new();

        public List<DialogueAction> OnEnterActions = new();

        [Tooltip("Если пусто - кнопка 'Продолжить'")]
        public List<DialogueChoice> Choices = new();

        [Header("Node Settings")]
        [Tooltip("Поставь галочку, если после этого текста диалог должен закончиться (если нет выборов)")]
        public bool EndDialogueHere = false; // <-- НОВОЕ ПОЛЕ
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string ChoiceText;
        public int NextNodeIndex;

        public List<DialogueCondition> Conditions = new();
        public List<DialogueAction> Actions = new();
    }
}