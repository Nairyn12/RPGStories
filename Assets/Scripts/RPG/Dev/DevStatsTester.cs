using UnityEngine;

namespace Game.RPG
{
    public class DevStatsTester : MonoBehaviour
    {
        [Header("Ссылки на системы (перетащить из Инспектора)")]
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private SkillDefinitionSO testSkillDef;
        [SerializeField] private PlayerLeveling playerLeveling;
        [SerializeField] private PlayerHealth playerHealth;     // ← ДОБАВЛЕНО
        [SerializeField] private SaveManager saveManager;       // ← ДОБАВЛЕНО
        [SerializeField] private PlayerStamina playerStamina;

        [Header("Тестовые настройки модификатора")]
        [SerializeField] private StatType testStat = StatType.Acrobatics;
        [SerializeField] private float testValue = 1f;
        [SerializeField] private ModifierType testType = ModifierType.Flat;

        private int _testCounter;

        private void OnGUI()
        {
            // Увеличили высоту панели, чтобы влезли новые кнопки
            GUILayout.BeginArea(new Rect(10, 10, 320, 800), GUI.skin.box);

            GUILayout.Label($"Level: {playerStats.Level}   SP: {playerStats.SkillPoints}");
            GUILayout.Label($"XP: {playerLeveling.CurrentXp} / {playerLeveling.XpForNextLevel} ({playerLeveling.XpProgress01 * 100f:F0}%)");

            // Проверка здоровья
            if (playerHealth != null)
                GUILayout.Label($"HP: {playerHealth.CurrentHealth:F0} / {playerHealth.MaxHealth:F0}");
            else
                GUILayout.Label("HP: (PlayerHealth не назначен!)");

            GUILayout.Space(6);

            GUILayout.Label($"Acrobatics: {playerStats.GetStat(StatType.Acrobatics):F1}");
            GUILayout.Label($"Persuasion: {playerStats.GetStat(StatType.Persuasion):F1}");
            GUILayout.Label($"Speed: {playerStats.GetStat(StatType.Speed):F1}");
            GUILayout.Space(6);

            if (GUILayout.Button("+1 Skill Point"))
                playerStats.AddSkillPoints(1);

            if (GUILayout.Button("Spend SP on Acrobatics"))
            {
                bool ok = playerStats.TrySpendSkillPoint(testSkillDef);
                Debug.Log($"[Dev] Spend success={ok}");
            }

            if (GUILayout.Button("Add test modifier"))
                AddTestModifier();

            if (GUILayout.Button("Remove test modifiers"))
                RemoveTestModifiers();

            GUILayout.Space(6);
            GUILayout.Label("— Level & XP —");
            if (GUILayout.Button("Level up (direct)")) playerStats.LevelUp();
            if (GUILayout.Button("+100 XP")) playerLeveling.AddXp(100);
            if (GUILayout.Button("+1000 XP")) playerLeveling.AddXp(1000);

            GUILayout.Space(6);
            GUILayout.Label("— Health —");
            if (GUILayout.Button("Take 20 Damage") && playerHealth != null) playerHealth.TakeDamage(20f);
            if (GUILayout.Button("Heal 20 HP") && playerHealth != null) playerHealth.Heal(20f);

            GUILayout.Space(6);
            GUILayout.Label("— Stamina —");
            if (playerStamina != null)
            {
                GUILayout.Label($"Stamina: {playerStamina.CurrentStamina:F1} / {playerStamina.MaxStamina:F1}");
                if (GUILayout.Button("Consume 10 Stamina")) playerStamina.TryConsume(10f);
            }

            GUILayout.Space(6);
            GUILayout.Label("— Skill checks —");
            if (GUILayout.Button("Check Acrobatics (Medium)"))
                Debug.Log($"[Dev] {SkillCheckService.Evaluate(StatType.Acrobatics, CheckDifficulty.Medium)}");

            if (GUILayout.Button("Check Persuasion (Hard)"))
                Debug.Log($"[Dev] {SkillCheckService.Evaluate(StatType.Persuasion, CheckDifficulty.Hard)}");

            if (GUILayout.Button("Dump all stats"))
                DumpStats();

            GUILayout.Space(6);
            GUILayout.Label("— Save System —");
            if (GUILayout.Button("SAVE GAME") && saveManager != null) saveManager.SaveGame();
            if (GUILayout.Button("LOAD GAME") && saveManager != null) saveManager.LoadGame();

            GUILayout.EndArea();
        }

        private void AddTestModifier()
        {
            var m = new StatModifier(testStat, testType, testValue, $"test_mod_{_testCounter++}", ModifierSource.TemporaryEffect);
            playerStats.AddModifier(m);
            Debug.Log($"[Dev] Added {m}. New value = {playerStats.GetStat(testStat):F2}");
        }

        private void RemoveTestModifiers()
        {
            var all = playerStats.GetAllModifiers();
            for (int i = all.Count - 1; i >= 0; i--)
                if (all[i].SourceId.StartsWith("test_mod_"))
                    playerStats.RemoveModifiersBySource(all[i].SourceId);

            Debug.Log($"[Dev] Removed test modifiers. New value = {playerStats.GetStat(testStat):F2}");
        }

        private void DumpStats()
        {
            var sb = new System.Text.StringBuilder("=== Player Stats ===\n");
            foreach (StatType s in System.Enum.GetValues(typeof(StatType)))
            {
                if (s == StatType.None) continue;
                sb.AppendLine($"{s,-15} = {playerStats.GetStat(s):F2}");
            }
            Debug.Log(sb.ToString());
        }
    }
}