using System;

namespace Game.RPG
{
    public enum ModifierType { Flat, Percent }

    public enum ModifierSource
    {
        Equipment,
        PermanentBoost,
        TemporaryEffect,
        QuestReward,
        LevelUp,
        Other
    }

    /// <summary>
    /// Неизменяемый модификатор стата.
    /// Формула: Total = (Base + ΣFlat) × (1 + ΣPercent)
    /// </summary>
    [Serializable]
    public struct StatModifier
    {
        public StatType Stat;
        public ModifierType Type;
        public float Value;
        public string SourceId;
        public ModifierSource Source;

        public StatModifier(StatType stat, ModifierType type, float value,
                            string sourceId, ModifierSource source = ModifierSource.Other)
        {
            Stat = stat;
            Type = type;
            Value = value;
            SourceId = sourceId;
            Source = source;
        }

        public override string ToString()
            => $"{Stat} +{Value}{(Type == ModifierType.Percent ? "%" : "")} (src: {SourceId})";
    }
}