using UnityEngine;

namespace Game.RPG
{
    public enum ItemType { Weapon, Armor, Consumable, Quest, Misc }

    public abstract class ItemDataSO : ScriptableObject
    {
        [Header("Base Info")]
        [Tooltip("Уникальный ID для сохранений (например, iron_sword). Не меняй после создания!")]
        public string ItemId;
        public string ItemName;
        [TextArea] public string Description;
        public Sprite Icon;

        [Header("Inventory Rules")]
        public ItemType Type;
        [Tooltip("Максимум предметов в одном слоте (для зелий - 20, для мечей - 1)")]
        public int MaxStack = 1;
    }
}
