using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "RPG/Weapon Data")]
    public class WeaponDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string WeaponName = "Sword";

        [Header("Combat Stats")]
        [Tooltip("Базовый урон оружия")]
        public float BaseDamage = 10f;

        [Tooltip("Время между ударами (в секундах)")]
        public float AttackCooldown = 0.8f;

        [Tooltip("Длина хитбокса (дистанция удара)")]
        public float AttackRange = 1.5f;

        [Tooltip("Стоимость атаки в стамине (0, если обычные атаки бесплатные)")]
        public float StaminaCost = 0f;

        [Header("Scaling (Прокачка)")]
        [Tooltip("Насколько процентов растет урон за 1 единицу навыка Melee (0.05 = 5%)")]
        public float MeleeScaling = 0.05f;

        /// <summary>
        /// Вычисляет итоговый урон с учетом характеристик игрока.
        /// </summary>
        public float CalculateDamage(PlayerStats stats)
        {
            float meleeSkill = stats.GetStat(StatType.Melee);
            // Формула: Базовый урон * (1 + (Навык Меча * Процент))
            float finalDamage = BaseDamage * (1f + (meleeSkill * MeleeScaling));
            return finalDamage;
        }
    }
}