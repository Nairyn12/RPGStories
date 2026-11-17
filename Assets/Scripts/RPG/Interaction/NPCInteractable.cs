using UnityEngine;

namespace Game.RPG
{
    public class NPCInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueSO dialogue;
        [SerializeField] private FactionDataSO faction; // <-- НОВОЕ

        public string GetInteractionText()
        {
            return $"Поговорить с {dialogue.NpcName}";
        }

        public void Interact(GameObject interactor)
        {
            // Передаем фракцию в Runner
            DialogueRunner.Instance.StartDialogue(dialogue, interactor, faction);
        }
    }
}