using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Enemy_", menuName = "RPG/Enemy Data")]
    public class EnemyDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string EnemyName = "Wolf";
        public int EnemyLevel = 1; // Для отображения над головой (например, "Волк [1]")

        [Header("Stats")]
        public float MaxHealth = 50f;
        public float MovementSpeed = 3.5f;

        [Header("Combat")]
        public float Damage = 10f;
        public float AttackRange = 1.5f;
        public float AttackCooldown = 1.5f;

        [Header("Detection")]
        [Tooltip("Радиус обнаружения игрока")]
        public float DetectionRadius = 8f;
        [Tooltip("Угол обзора (например, 120 градусов)")]
        public float FieldOfView = 120f;

        [Header("Rewards")]
        [Tooltip("Сколько опыта дает этот враг (фиксированное число)")]
        public int XpReward = 50;
    }
}