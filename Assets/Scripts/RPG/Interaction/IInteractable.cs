using UnityEngine;

namespace Game.RPG
{
    public interface IInteractable
    {
        string GetInteractionText(); // Например: "Подобрать Ржавый меч"
        void Interact(GameObject interactor);
    }
}