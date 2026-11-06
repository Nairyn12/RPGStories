using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RPG
{
    public class PlayerStats : MonoBehaviour, ISaveable
    {
        [Header("Base Resources")]
        [SerializeField] private float baseHealth = 100f;
        [SerializeField] private float baseStamina = 50f;
        [SerializeField] private float baseMana = 30f;
        [SerializeField] private float baseSpeed = 5f;

        [Header("Per-Level Growth")]
        [SerializeField] private float healthPerLevel = 10f;
        [SerializeField] private float staminaPerLevel = 5f;
        [SerializeField] private float manaPerLevel = 5f;

        [Header("Magic Scaling")]
        [Tooltip("Сколько маны дает 1 уровень любого магического навыка")]
        [SerializeField] private float manaPerMagicPoint = 2f; // <-- ДОБАВИТЬ

        [Header("Level")]
        [SerializeField] private int level = 1;
        [SerializeField] private int skillPoints = 0;
        [SerializeField] private int startingSkillPoints = 0;

        private readonly Dictionary<StatType, int> _baseSkillValues = new();
        private readonly List<StatModifier> _modifiers = new();

        public event Action OnStatsChanged;
        public event Action<StatType, float> OnStatChanged;

        public int Level => level;
        public int SkillPoints => skillPoints;

        private void Awake()
        {
            skillPoints = startingSkillPoints;
            SkillCheckService.Register(this);
        }

        private void OnDestroy() => SkillCheckService.Unregister(this);

        public float GetStat(StatType stat)
        {
            float baseVal = stat.IsResource() ? GetResourceBase(stat) : GetBaseSkillValue(stat);
            float flat = 0f, percent = 0f;
            foreach (var m in _modifiers)
            {
                if (m.Stat != stat) continue;
                if (m.Type == ModifierType.Flat) flat += m.Value;
                else percent += m.Value;
            }
            return (baseVal + flat) * (1f + percent);
        }

        public int GetBaseSkillValue(StatType skill) => _baseSkillValues.TryGetValue(skill, out var v) ? v : 0;

        private float GetResourceBase(StatType resource) => resource switch
        {
            StatType.HealthMax => baseHealth + (level - 1) * healthPerLevel,
            StatType.StaminaMax => baseStamina + (level - 1) * staminaPerLevel,

            // Мана зависит от уровня и от суммы всех магических навыков
            StatType.ManaMax => baseMana + (level - 1) * manaPerLevel + (GetTotalMagicSkill() * manaPerMagicPoint),

            StatType.Speed => baseSpeed,
            _ => 0f
        };

        // НОВЫЙ МЕТОД
        public int GetTotalMagicSkill()
        {
            return GetBaseSkillValue(StatType.Elemental)
                 + GetBaseSkillValue(StatType.Restoration)
                 + GetBaseSkillValue(StatType.Manipulation);
        }

        public void AddModifier(StatModifier m) { _modifiers.Add(m); NotifyStatChanged(m.Stat); }
        public void RemoveModifiersBySource(string sourceId)
        {
            if (string.IsNullOrEmpty(sourceId)) return;
            bool removed = false;
            for (int i = _modifiers.Count - 1; i >= 0; i--)
                if (_modifiers[i].SourceId == sourceId) { _modifiers.RemoveAt(i); removed = true; }
            if (removed) OnStatsChanged?.Invoke();
        }

        // ▼▼▼ ДОБАВЬ ВОТ ЭТУ СТРОКУ ▼▼▼
        public IReadOnlyList<StatModifier> GetAllModifiers() => _modifiers;
        // ▲▲▲ ---------------------- ▲▲▲

        public bool TrySpendSkillPoint(StatType skill, int cost, int maxValue)
        {
            if (!skill.IsSkill() || cost <= 0 || skillPoints < cost) return false;
            int current = GetBaseSkillValue(skill);
            if (current >= maxValue) return false;
            skillPoints -= cost; _baseSkillValues[skill] = current + 1;
            NotifyStatChanged(skill);
            // Если прокачали магию - пересчитываем максимальную ману
            if (skill.IsMagicSkill())
            {
                NotifyStatChanged(StatType.ManaMax);
            }
            return true;
        }

        public bool TrySpendSkillPoint(SkillDefinitionSO skillDef)
        {
            if (skillDef == null) return false;
            int current = GetBaseSkillValue(skillDef.Stat);
            int cost = skillDef.GetCostForNextPoint(current);
            return TrySpendSkillPoint(skillDef.Stat, cost, skillDef.MaxValue);
        }

        public void AddSkillPoints(int amount) { if (amount > 0) { skillPoints += amount; OnStatsChanged?.Invoke(); } }

        public void LevelUp(int pointsGranted = 1)
        {
            level++; skillPoints += pointsGranted;
            NotifyStatChanged(StatType.HealthMax);
            NotifyStatChanged(StatType.StaminaMax);
            NotifyStatChanged(StatType.ManaMax);
            OnStatsChanged?.Invoke();
        }

        // ==== ISaveable ====
        public string SaveKey => "PlayerStats";
        public string CaptureState()
        {
            var state = new RPGState { Level = level, SkillPoints = skillPoints };
            foreach (var kv in _baseSkillValues) state.BaseSkills.Add(new SkillEntry { Stat = kv.Key, Value = kv.Value });
            foreach (var m in _modifiers)
                if (m.Source is ModifierSource.PermanentBoost or ModifierSource.QuestReward)
                    state.PermanentModifiers.Add(new ModifierEntry { Stat = m.Stat, Type = m.Type, Value = m.Value, SourceId = m.SourceId, Source = m.Source });
            return JsonUtility.ToJson(state);
        }

        public void RestoreState(string json)
        {
            var state = JsonUtility.FromJson<RPGState>(json);
            if (state == null) return;
            _baseSkillValues.Clear(); _modifiers.Clear();
            level = state.Level; skillPoints = state.SkillPoints;
            foreach (var e in state.BaseSkills) _baseSkillValues[e.Stat] = e.Value;
            foreach (var m in state.PermanentModifiers) _modifiers.Add(new StatModifier(m.Stat, m.Type, m.Value, m.SourceId, m.Source));
            OnStatsChanged?.Invoke();
        }

        private void NotifyStatChanged(StatType stat)
        {
            OnStatChanged?.Invoke(stat, GetStat(stat));
            OnStatsChanged?.Invoke();
        }
    }

    [Serializable]
    public class RPGState
    {
        public int Level; public int SkillPoints;
        public List<SkillEntry> BaseSkills = new(); public List<ModifierEntry> PermanentModifiers = new();
    }
    [Serializable] public class SkillEntry { public StatType Stat; public int Value; }
    [Serializable] public class ModifierEntry { public StatType Stat; public ModifierType Type; public float Value; public string SourceId; public ModifierSource Source; }
}
