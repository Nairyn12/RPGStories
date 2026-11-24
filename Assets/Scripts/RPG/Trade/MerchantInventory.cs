using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.RPG
{
    public class MerchantInventory : MonoBehaviour, ISaveable
    {
        [Header("Faction & Settings")]
        public FactionDataSO Faction;

        [Header("Starting Stats")]
        public int StartGold = 500;
        public List<PlayerInventory.InventorySlot> StartItems = new();

        [Tooltip("Требуемый навык Убеждения для покупки уникальных вещей")]
        public int RequiredPersuasionForUnique = 5;

        private int _currentGold;
        private List<PlayerInventory.InventorySlot> _items = new();

        public int Gold => _currentGold;
        public IReadOnlyList<PlayerInventory.InventorySlot> Items => _items;

        public event Action OnMerchantChanged;

        private void Awake()
        {
            _currentGold = StartGold;
            foreach (var item in StartItems)
            {
                if (item.Item != null)
                    _items.Add(new PlayerInventory.InventorySlot { Item = item.Item, Amount = item.Amount });
            }
        }

        public void AddGold(int amount) { _currentGold += amount; OnMerchantChanged?.Invoke(); }
        public void RemoveGold(int amount) { _currentGold = Mathf.Max(0, _currentGold - amount); OnMerchantChanged?.Invoke(); }

        public void AddItem(ItemDataSO item, int amount)
        {
            var existing = _items.FirstOrDefault(s => s.Item == item);
            if (existing != null && item.MaxStack > 1)
            {
                existing.Amount += amount;
            }
            else
            {
                _items.Add(new PlayerInventory.InventorySlot { Item = item, Amount = amount });
            }
            OnMerchantChanged?.Invoke();
        }

        public bool RemoveItem(PlayerInventory.InventorySlot slot, int amount)
        {
            if (!_items.Contains(slot)) return false;
            slot.Amount -= amount;
            if (slot.Amount <= 0) _items.Remove(slot);
            OnMerchantChanged?.Invoke();
            return true;
        }

        // ==== СОХРАНЕНИЕ ====
        public string SaveKey => $"Merchant_{gameObject.name}";

        [Serializable]
        private class SaveData
        {
            public int Gold;
            public List<string> ItemIds = new();
            public List<int> Amounts = new();
        }

        public string CaptureState()
        {
            var data = new SaveData { Gold = _currentGold };
            foreach (var slot in _items)
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

            _currentGold = data.Gold;
            _items.Clear();
            var allItems = Resources.LoadAll<ItemDataSO>("Items");

            for (int i = 0; i < data.ItemIds.Count; i++)
            {
                var item = allItems.FirstOrDefault(it => it.ItemId == data.ItemIds[i]);
                if (item != null)
                {
                    _items.Add(new PlayerInventory.InventorySlot { Item = item, Amount = data.Amounts[i] });
                }
            }
            OnMerchantChanged?.Invoke();
        }
    }
}