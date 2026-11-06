using System.Collections.Generic;
using UnityEngine;

namespace Game.RPG
{
    public class PlayerInventory : MonoBehaviour, ISaveable
    {
        // Временный класс для хранения предметов в слоте
        [System.Serializable]
        public class InventorySlot
        {
            public ItemDataSO Item;
            public int Amount;
        }

        [SerializeField] private List<InventorySlot> _slots = new();

        public event System.Action OnInventoryChanged;

        public IReadOnlyList<InventorySlot> Slots => _slots;

        public void AddItem(ItemDataSO item, int amount = 1)
        {
            if (item == null || amount <= 0) return;

            if (item.MaxStack > 1)
            {
                foreach (var slot in _slots)
                {
                    if (slot.Item == item && slot.Amount < item.MaxStack)
                    {
                        slot.Amount += amount;
                        Debug.Log($"[Inventory] Добавлено {amount}x {item.ItemName}. Всего: {slot.Amount}");
                        OnInventoryChanged?.Invoke(); // <-- ДОБАВИТЬ ВОТ ЭТУ СТРОКУ
                        return;
                    }
                }
            }

            InventorySlot newSlot = new InventorySlot { Item = item, Amount = amount };
            _slots.Add(newSlot);
            Debug.Log($"[Inventory] Получен новый предмет: {item.ItemName} ({amount} шт.)");
            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(ItemDataSO item, int amount = 1)
        {
            for (int i = _slots.Count - 1; i >= 0; i--)
            {
                if (_slots[i].Item == item)
                {
                    _slots[i].Amount -= amount;
                    if (_slots[i].Amount <= 0)
                        _slots.RemoveAt(i);

                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }

        // ==== ISaveable (Временная заглушка, сделаем позже) ====
        public string SaveKey => "PlayerInventory";
        public string CaptureState() => "{}"; // Позже здесь будет JSON
        public void RestoreState(string json) { }
    }
}