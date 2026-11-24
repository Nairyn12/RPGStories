using UnityEngine;

namespace Game.RPG
{
    public enum ItemType { Weapon, Armor, Consumable, Quest, Misc }

    // НОВЫЙ ENUM
    public enum EquipmentType { None, Weapon, Armor, Helmet }

    public abstract class ItemDataSO : ScriptableObject
    {
        [Header("Base Info")]
        public string ItemId;
        public string ItemName;
        [TextArea] public string Description;
        public Sprite Icon;

        [Header("Trade Settings")]
        public int BasePrice = 10;
        public bool IsUnique = false; // Уникальные предметы (требуют Persuasion)

        [Header("World Representation")] // <-- НОВОЕ
        [Tooltip("Префаб, который спавнится, если выбросить предмет на землю")]
        public GameObject WorldPrefab;

        [Header("Inventory Rules")]
        public ItemType Type;
        public int MaxStack = 1;

        [Tooltip("Можно ли положить этот предмет в панель быстрого доступа (хотбар)?")]
        public bool CanBeInHotbar = true; // <-- НОВОЕ ПОЛЕ

        [Header("Equipment")] // <-- ДОБАВИТЬ
        public EquipmentType EquipSlot = EquipmentType.None;        
    }
}