using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(Collider))]
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemDataSO itemData;
        [SerializeField] private int amount = 1;

        public string GetInteractionText()
        {
            return $"Подобрать {itemData.ItemName}";
        }

        public void Interact(GameObject interactor)
        {
            if (interactor.TryGetComponent(out PlayerInventory inventory))
            {
                inventory.AddItem(itemData, amount);
                Destroy(gameObject); // Уничтожаем предмет с земли
            }
        }
    }
}