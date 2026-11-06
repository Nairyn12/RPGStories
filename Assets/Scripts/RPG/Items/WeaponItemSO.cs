using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "RPG/Items/Weapon")]
    public class WeaponItemSO : ItemDataSO
    {
        [Header("Combat Stats")]
        public float BaseDamage = 10f;
        public float AttackCooldown = 0.8f;
        public float AttackRange = 1.5f;
        public float StaminaCost = 0f;

        [Header("Scaling")]
        [Tooltip("Насколько процентов растет урон за 1 единицу навыка Melee")]
        public float MeleeScaling = 0.05f;

        public float CalculateDamage(PlayerStats stats)
        {
            float meleeSkill = stats.GetStat(StatType.Melee);
            return BaseDamage * (1f + (meleeSkill * MeleeScaling));
        }
    }
}
