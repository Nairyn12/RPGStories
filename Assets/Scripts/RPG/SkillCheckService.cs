using UnityEngine;

namespace Game.RPG
{
    /// <summary>Стандартные уровни сложности. Используйте их, а не магические числа.</summary>
    public enum CheckDifficulty
    {
        Trivial = 1,
        Easy = 3,
        Medium = 5,
        Hard = 8,
        VeryHard = 12,
        Impossible = 99
    }

    public struct SkillCheckResult
    {
        public StatType Skill;
        public float SkillValue;
        public int Difficulty;
        public float Margin;
        public bool Success;

        public override string ToString() =>
            $"{Skill} {SkillValue:F1} vs {Difficulty} → " +
            $"{(Success ? "PASS" : "FAIL")} (margin {Margin:+0.0;-0.0})";
    }

    /// <summary>
    /// Единая точка проверки навыков. Регистрируется из PlayerStats.Awake.
    /// Детерминированные проверки: skill >= difficulty → pass.
    /// </summary>
    public static class SkillCheckService
    {
        private static PlayerStats _player;

        public static bool VerboseLogging = false;

        public static void Register(PlayerStats player) => _player = player;
        public static void Unregister(PlayerStats player)
        {
            if (_player == player) _player = null;
        }

        // --- Удобные перегрузки ---

        public static bool Check(StatType skill, CheckDifficulty difficulty)
            => Check(skill, (int)difficulty);

        public static bool Check(StatType skill, int difficulty)
            => Evaluate(skill, difficulty).Success;

        public static SkillCheckResult Evaluate(StatType skill, CheckDifficulty difficulty)
            => Evaluate(skill, (int)difficulty);

        public static SkillCheckResult Evaluate(StatType skill, int difficulty)
        {
            float value = _player != null ? _player.GetStat(skill) : 0f;
            float margin = value - difficulty;

            var result = new SkillCheckResult
            {
                Skill = skill,
                SkillValue = value,
                Difficulty = difficulty,
                Margin = margin,
                Success = margin >= 0f
            };

            if (VerboseLogging) Debug.Log($"[SkillCheck] {result}");
            return result;
        }
    }
}