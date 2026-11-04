using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Skill_", menuName = "RPG/Skill Definition")]
    public class SkillDefinitionSO : ScriptableObject
    {
        [Header("Identity")]
        public StatType Stat;
        [Tooltip("Стабильный строковый id для сейвов и условий. Должен совпадать между билдами.")]
        public string Id;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Icon;

        [Header("Progression")]
        public SkillBranchSO Branch;
        [Range(1, 30)] public int MaxValue = 20;

        [Header("Skill Point Cost (в очках)")]
        public int CostLow = 1;   // значение 0-9
        public int CostMid = 2;   // значение 10-14
        public int CostHigh = 3;   // значение 15+

        [Header("Tags (для проверок и гейтинга)")]
        public string[] CheckTags = { };

        public int GetCostForNextPoint(int currentValue)
        {
            if (currentValue < 10) return CostLow;
            if (currentValue < 15) return CostMid;
            return CostHigh;
        }
    }
}
