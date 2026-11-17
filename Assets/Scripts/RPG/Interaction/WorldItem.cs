using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(Collider))]
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemDataSO itemData;
        [SerializeField] private int amount = 1;

        // Вот та самая переменная, которой не хватало
        private bool _isCollected = false;

        // НОВЫЙ МЕТОД: Вызывается при спавне предмета из инвентаря
        public void Setup(ItemDataSO item, int itemAmount)
        {
            itemData = item;
            amount = itemAmount;
        }

        public string GetInteractionText()
        {
            return $"Подобрать {itemData.ItemName}";
        }

        public void Interact(GameObject interactor)
        {
            if (_isCollected) return;
            _isCollected = true;

            if (interactor.TryGetComponent(out PlayerInventory inventory) ||
                (inventory = FindFirstObjectByType<PlayerInventory>()) != null)
            {
                inventory.AddItem(itemData, amount);

                // Отключаем коллайдер, чтобы луч больше не видел этот объект в этом кадре
                if (TryGetComponent(out Collider col)) col.enabled = false;

                Destroy(gameObject);
            }
        }
    }
}