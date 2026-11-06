using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.RPG
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerHealth health;
        [SerializeField] private PlayerStamina stamina;
        [SerializeField] private PlayerLeveling leveling;
        [SerializeField] private PlayerMana mana;

        [Header("UI Elements")]
        [SerializeField] private Image healthFill;
        [SerializeField] private Image staminaFill;
        [SerializeField] private Image manaFill;
        [SerializeField] private Image xpFill;
        [SerializeField] private TextMeshProUGUI levelText;

        private void Start()
        {
            if (health == null || stamina == null || leveling == null) return;

            // Подписываемся на события
            health.OnHealthChanged += UpdateHealth;
            stamina.OnStaminaChanged += UpdateStamina;
            leveling.OnXpChanged += UpdateXp;
            leveling.OnLevelUp += UpdateLevel;

            // Инициализируем при старте
            UpdateHealth(health.CurrentHealth, health.MaxHealth);
            UpdateStamina(stamina.CurrentStamina, stamina.MaxStamina);
            UpdateXp(leveling.CurrentXp, leveling.XpForNextLevel);
            UpdateLevel(leveling.MaxLevel > 0 ? leveling.CurrentXp : 1); // Просто передаем текущий уровень
            levelText.text = $"Level {health.GetComponent<PlayerStats>().Level}";

            mana.OnManaChanged += UpdateMana;
            UpdateMana(mana.CurrentMana, mana.MaxMana);
        }

        private void OnDestroy()
        {
            // Отписываемся, чтобы не было утечек памяти
            if (health != null) health.OnHealthChanged -= UpdateHealth;
            if (stamina != null) stamina.OnStaminaChanged -= UpdateStamina;
            if (leveling != null) leveling.OnXpChanged -= UpdateXp;
            if (leveling != null) leveling.OnLevelUp -= UpdateLevel;
            if (mana != null) mana.OnManaChanged -= UpdateMana;
        }

        private void UpdateHealth(float current, float max)
        {
            healthFill.fillAmount = current / max;
        }

        private void UpdateStamina(float current, float max)
        {
            staminaFill.fillAmount = current / max;
        }

        private void UpdateMana(float current, float max)
        {
            manaFill.fillAmount = current / max;
        }

        private void UpdateXp(int current, int required)
        {
            xpFill.fillAmount = (float)current / required;
        }

        private void UpdateLevel(int newLevel)
        {
            // Обновляем текст при повышении уровня
            var stats = health.GetComponent<PlayerStats>();
            if (stats != null) levelText.text = $"Level {stats.Level}";
        }
    }
}