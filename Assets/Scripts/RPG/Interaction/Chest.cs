using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(Collider))]
    public class Chest : MonoBehaviour, IInteractable, ISaveable
    {
        [Header("Loot Settings")]
        [SerializeField] private List<PlayerInventory.InventorySlot> startLoot = new();

        private List<PlayerInventory.InventorySlot> _slots = new();

        public IReadOnlyList<PlayerInventory.InventorySlot> Slots => _slots;
        public event Action OnChestChanged;

        private void Awake()
        {
            foreach (var slot in startLoot)
            {
                if (slot.Item != null)
                {
                    _slots.Add(new PlayerInventory.InventorySlot
                    {
                        Item = slot.Item,
                        Amount = slot.Amount
                    });
                }
            }
        }

        public string GetInteractionText() => "Открыть сундук";

        public void Interact(GameObject interactor)
        {
            // 🛠 Проверка: если на объекте есть LockedObject и он заперт - не открываем
            var lockedObj = GetComponent<LockedObject>();
            if (lockedObj != null && lockedObj.IsLocked())
            {
                Debug.Log("[Chest] Сундук заперт! Сначала взломайте замок.");
                return;
            }

            var chestUI = FindFirstObjectByType<ChestUI>();
            if (chestUI != null)
            {
                chestUI.Open(this);
            }
        }

        public void AddItem(ItemDataSO item, int amount)
        {
            if (item == null || amount <= 0) return;

            if (item.MaxStack > 1)
            {
                foreach (var slot in _slots)
                {
                    if (slot.Item == item && slot.Amount < item.MaxStack)
                    {
                        slot.Amount += amount;
                        OnChestChanged?.Invoke();
                        return;
                    }
                }
            }

            _slots.Add(new PlayerInventory.InventorySlot { Item = item, Amount = amount });
            OnChestChanged?.Invoke();
        }

        public bool RemoveItem(PlayerInventory.InventorySlot slot, int amount = 1)
        {
            if (slot == null || !_slots.Contains(slot)) return false;

            slot.Amount -= amount;
            if (slot.Amount <= 0) _slots.Remove(slot);

            OnChestChanged?.Invoke();
            return true;
        }

        // ==== СОХРАНЕНИЕ ====
        public string SaveKey => $"Chest_{gameObject.name}_{transform.position.x}_{transform.position.z}";

        [Serializable]
        private class SaveData
        {
            public List<string> ItemIds = new();
            public List<int> Amounts = new();
        }

        public string CaptureState()
        {
            var data = new SaveData();
            foreach (var slot in _slots)
            {
                if (slot.Item != null)
                {
                    data.ItemIds.Add(slot.Item.ItemId);
                    data.Amounts.Add(slot.Amount);
                }
            }
            return JsonUtility.ToJson(data);
        }

        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<SaveData>(json);
            if (data == null) return;

            _slots.Clear();
            var allItems = Resources.LoadAll<ItemDataSO>("Items");
            for (int i = 0; i < data.ItemIds.Count; i++)
            {
                var item = allItems.FirstOrDefault(it => it.ItemId == data.ItemIds[i]);
                if (item != null)
                {
                    _slots.Add(new PlayerInventory.InventorySlot
                    {
                        Item = item,
                        Amount = data.Amounts[i]
                    });
                }
            }
            OnChestChanged?.Invoke();
        }
    }
}