namespace Game.RPG
{
    /// <summary>
    /// Все статы игры — ресурсы (HealthMax и т.д.) и навыки.
    ///
    /// ВАЖНО:
    /// - НИКОГДА не переупорядочивать и не перенумеровывать существующие значения
    ///   (они сериализуются в сейвах и ассетах как int).
    /// - ВСЕГДА добавлять новые значения в конец своей группы, следующим свободным номером.
    /// - Группы разделены с запасом (0-99 ресурсы, 100-199 ловкость, ...).
    /// </summary>
    public enum StatType
    {
        None = -1,

        // ===== Resources (0-99) =====
        HealthMax = 0,
        StaminaMax = 1,
        ManaMax = 2,
        Speed = 3,
        // 4-99 зарезервировано

        // ===== Agility skills (100-199) =====
        Acrobatics = 100,
        Perception = 101,
        Stealth = 102,
        Reaction = 103,
        // 104-199 зарезервировано

        // ===== Combat skills (200-299) =====
        Melee = 200,
        Ranged = 201,
        Defense = 202,

        // ===== Magic skills (300-399) =====
        Elemental = 300,
        Restoration = 301,
        Manipulation = 302,

        // ===== Social skills (400-499) =====
        Persuasion = 400,
        Deception = 401,
        Trade = 402,

        // ===== Endurance skills (500-599) =====
        Resilience = 500,
        Regeneration = 501,
        Breathing = 502,
    }

    public static class StatTypeExtensions
    {
        public static bool IsResource(this StatType s) => (int)s >= 0 && (int)s < 100;
        public static bool IsSkill(this StatType s) => (int)s >= 100;
        public static bool IsAgilitySkill(this StatType s) => (int)s >= 100 && (int)s < 200;
        public static bool IsCombatSkill(this StatType s) => (int)s >= 200 && (int)s < 300;
        public static bool IsMagicSkill(this StatType s) => (int)s >= 300 && (int)s < 400;
        public static bool IsSocialSkill(this StatType s) => (int)s >= 400 && (int)s < 500;
        public static bool IsEnduranceSkill(this StatType s) => (int)s >= 500 && (int)s < 600;
    }
}
