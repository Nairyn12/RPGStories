using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.RPG
{
    public class PlayerInventory : MonoBehaviour, ISaveable
    {
        [Serializable]
        public class InventorySlot
        {
            public ItemDataSO Item;
            public int Amount;

            // Единый источник истины
            public EquipmentType EquippedSlot = EquipmentType.None;
            public int HotbarIndex = -1; // -1 значит не в хотбаре
        }

        [Header("References")]
        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerCombat combat;
        [SerializeField] private Transform playerTransform;

        [Header("Drop Settings")]
        [SerializeField] private GameObject genericLootBagPrefab;
        [SerializeField] private float dropDistance = 1.5f;

        [SerializeField] private List<InventorySlot> _slots = new();
        public IReadOnlyList<InventorySlot> Slots => _slots;

        public event Action OnInventoryChanged;
        public event Action OnEquipmentChanged;

        private void Awake()
        {
            if (stats == null) stats = GetComponent<PlayerStats>();
            if (combat == null) combat = GetComponent<PlayerCombat>();
            if (playerTransform == null) playerTransform = transform;

            // Подстраховка: если висят на разных объектах
            if (stats == null) stats = FindFirstObjectByType<PlayerStats>();
            if (combat == null) combat = FindFirstObjectByType<PlayerCombat>();
        }

        // ==== ДОБАВЛЕНИЕ / УДАЛЕНИЕ ====
        public void AddItem(ItemDataSO item, int amount = 1)
        {
            if (item == null || amount <= 0) return;

            bool added = false;

            if (item.MaxStack > 1)
            {
                foreach (var slot in _slots)
                {
                    if (slot.Item != null && slot.Item.ItemId == item.ItemId && slot.Amount < item.MaxStack)
                    {
                        slot.Amount += amount;
                        added = true;
                        break;
                    }
                }
            }

            if (!added)
            {
                _slots.Add(new InventorySlot { Item = item, Amount = amount });
            }

            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.ReportCollect(item.ItemId, amount);
            }

            OnInventoryChanged?.Invoke();
        }

        public bool RemoveItem(InventorySlot slot, int amount = 1)
        {
            if (slot == null) return false;

            if (slot.EquippedSlot != EquipmentType.None) UnequipItem(slot.EquippedSlot);
            if (slot.HotbarIndex != -1) ClearQuickSlot(slot.HotbarIndex);

            slot.Amount -= amount;
            if (slot.Amount <= 0) _slots.Remove(slot);

            OnInventoryChanged?.Invoke();
            return true;
        }

        // ==== ЭКИПИРОВКА ====
        public void EquipItem(InventorySlot targetSlot)
        {
            if (targetSlot == null || targetSlot.Item == null || targetSlot.Item.EquipSlot == EquipmentType.None) return;

            EquipmentType type = targetSlot.Item.EquipSlot;

            var currentEquipped = _slots.Find(s => s.EquippedSlot == type);

            if (currentEquipped != null && currentEquipped != targetSlot)
            {
                UnequipItem(type);
            }

            targetSlot.EquippedSlot = type;

            if (targetSlot.Item is ArmorItemSO armor)
            {
                foreach (var mod in armor.Modifiers) stats.AddModifier(mod);
            }
            else if (targetSlot.Item is WeaponItemSO weapon)
            {
                combat.EquipWeapon(weapon);
            }

            OnInventoryChanged?.Invoke();
            OnEquipmentChanged?.Invoke();
        }

        public void UnequipItem(EquipmentType type)
        {
            var equipped = _slots.Find(s => s.EquippedSlot == type);
            if (equipped == null) return;

            equipped.EquippedSlot = EquipmentType.None;

            if (equipped.Item is ArmorItemSO armor)
            {
                foreach (var mod in armor.Modifiers) stats.RemoveModifiersBySource(mod.SourceId);
            }
            else if (equipped.Item is WeaponItemSO)
            {
                combat.EquipWeapon(null);
            }

            OnInventoryChanged?.Invoke();
            OnEquipmentChanged?.Invoke();
        }

        public InventorySlot GetEquippedSlot(EquipmentType type)
        {
            return _slots.Find(s => s.EquippedSlot == type);
        }

        // ==== ХОТБАР ====
        public void SetQuickSlot(int index, InventorySlot targetSlot)
        {
            if (targetSlot == null || targetSlot.Item == null) return;
            ClearQuickSlot(index);
            if (targetSlot.HotbarIndex != -1) ClearQuickSlot(targetSlot.HotbarIndex);
            targetSlot.HotbarIndex = index;
            OnInventoryChanged?.Invoke();
        }

        public void ClearQuickSlot(int index)
        {
            var slot = _slots.Find(s => s.HotbarIndex == index);
            if (slot != null)
            {
                slot.HotbarIndex = -1;
                OnInventoryChanged?.Invoke();
            }
        }

        public InventorySlot GetQuickSlot(int index)
        {
            return _slots.Find(s => s.HotbarIndex == index);
        }

        public void UseQuickSlot(int index)
        {
            var slot = GetQuickSlot(index);
            if (slot == null) return;
            UseItem(slot);
        }

        // ==== ИСПОЛЬЗОВАНИЕ ====
        public void UseItem(InventorySlot slot)
        {
            if (slot == null || slot.Item == null) return;

            if (slot.Item is ConsumableItemSO consumable)
            {
                consumable.Use(gameObject);
                RemoveItem(slot, 1);
            }
            else if (slot.Item.EquipSlot != EquipmentType.None)
            {
                if (slot.EquippedSlot != EquipmentType.None) UnequipItem(slot.EquippedSlot);
                else EquipItem(slot);
            }
        }

        // ==== ВЫБРАСЫВАНИЕ ====
        public void DropItem(InventorySlot slot, int amount = 1)
        {
            if (slot == null || slot.Item == null) return;

            if (slot.EquippedSlot != EquipmentType.None)
            {
                Debug.LogWarning("[Inventory] Нельзя выбросить надетый предмет!");
                return;
            }

            GameObject prefabToSpawn = slot.Item.WorldPrefab != null ? slot.Item.WorldPrefab : genericLootBagPrefab;

            if (prefabToSpawn != null && playerTransform != null)
            {
                Vector3 dropPosition = playerTransform.position + playerTransform.forward * dropDistance + Vector3.up * 0.5f;
                GameObject spawnedObj = Instantiate(prefabToSpawn, dropPosition, Quaternion.identity);

                var worldItem = spawnedObj.GetComponent<WorldItem>() ?? spawnedObj.AddComponent<WorldItem>();
                worldItem.Setup(slot.Item, 1);
            }

            RemoveItem(slot, 1);
        }

        // ==========================================
        // СОХРАНЕНИЕ И ЗАГРУЗКА (ISaveable)
        // ==========================================
        public string SaveKey => "PlayerInventory";

        [Serializable]
        private class SaveData
        {
            public List<string> ItemIds = new();
            public List<int> Amounts = new();
            public List<int> EquippedSlots = new();
            public List<int> HotbarIndexes = new();
        }

        public string CaptureState()
        {
            var data = new SaveData();
            foreach (var slot in _slots)
            {
                if (slot.Item != null)
                {
                    if (string.IsNullOrEmpty(slot.Item.ItemId))
                    {
                        Debug.LogError($"[Inventory] У предмета {slot.Item.name} ПУСТОЙ ItemId! Он не сохранится.");
                        continue;
                    }
                    data.ItemIds.Add(slot.Item.ItemId);
                    data.Amounts.Add(slot.Amount);
                    data.EquippedSlots.Add((int)slot.EquippedSlot);
                    data.HotbarIndexes.Add(slot.HotbarIndex);
                }
            }
            return JsonUtility.ToJson(data);
        }

        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<SaveData>(json);
            if (data == null || data.ItemIds == null) return;

            // 1. Снимаем старые модификаторы
            foreach (var slot in _slots)
            {
                if (slot.EquippedSlot != EquipmentType.None && slot.Item is ArmorItemSO armor && stats != null)
                {
                    foreach (var mod in armor.Modifiers) stats.RemoveModifiersBySource(mod.SourceId);
                }
            }

            _slots.Clear();

            // 2. Загружаем все предметы из Resources
            var allItems = Resources.LoadAll<ItemDataSO>("Items");

            // 3. Восстанавливаем слоты
            for (int i = 0; i < data.ItemIds.Count; i++)
            {
                string savedId = data.ItemIds[i];
                var item = allItems.FirstOrDefault(it => it.ItemId == savedId);

                if (item == null)
                {
                    Debug.LogError($"[Inventory] ОШИБКА: Не найден предмет с ID '{savedId}' в папке Resources/Items!");
                    continue;
                }

                var newSlot = new InventorySlot
                {
                    Item = item,
                    Amount = data.Amounts[i],
                    EquippedSlot = (EquipmentType)data.EquippedSlots[i],
                    HotbarIndex = data.HotbarIndexes[i]
                };
                _slots.Add(newSlot);

                // Возвращаем модификаторы
                if (newSlot.EquippedSlot != EquipmentType.None)
                {
                    if (item is ArmorItemSO armor && stats != null)
                    {
                        foreach (var mod in armor.Modifiers) stats.AddModifier(mod);
                    }
                    else if (item is WeaponItemSO weapon && combat != null)
                    {
                        combat.EquipWeapon(weapon);
                    }
                }
            }

            OnInventoryChanged?.Invoke();
            OnEquipmentChanged?.Invoke();
            Debug.Log($"[Inventory] Успешно загружено предметов: {_slots.Count}");
        }
    }
}