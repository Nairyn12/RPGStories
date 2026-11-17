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

        [Header("World Representation")] // <-- НОВОЕ
        [Tooltip("Префаб, который спавнится, если выбросить предмет на землю")]
        public GameObject WorldPrefab;

        [Header("Inventory Rules")]
        public ItemType Type;
        public int MaxStack = 1;

        [Header("Equipment")] // <-- ДОБАВИТЬ
        public EquipmentType EquipSlot = EquipmentType.None;
    }
}