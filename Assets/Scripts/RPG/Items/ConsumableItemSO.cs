using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Consumable_", menuName = "RPG/Items/Consumable")]
    public class ConsumableItemSO : ItemDataSO
    {
        [Header("Effects")]
        public float HealthRestore = 0f;
        public float StaminaRestore = 0f;
        public float ManaRestore = 0f;

        /// <summary>
        /// Применяет эффект предмета к игроку.
        /// </summary>
        public void Use(GameObject player)
        {
            if (HealthRestore > 0 && player.TryGetComponent(out PlayerHealth health))
                health.Heal(HealthRestore);

            if (StaminaRestore > 0 && player.TryGetComponent(out PlayerStamina stamina))
                stamina.RestoreStamina(StaminaRestore);

            if (ManaRestore > 0 && player.TryGetComponent(out PlayerMana mana))
                mana.RestoreMana(ManaRestore);

            Debug.Log($"[Item] Использовано: {ItemName}");
        }
    }
}
