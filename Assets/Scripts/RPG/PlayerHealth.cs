using System;
using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerHealth : MonoBehaviour, ISaveable
    {
        [SerializeField] private float currentHealth;
        private PlayerStats _stats;
        private float _lastMaxHealth;

        public event Action<float, float> OnHealthChanged; // (current, max)
        public event Action OnDeath;
        // Добавь рядом с другими событиями
        public event Action OnDamaged;

        private float _lastDamageTime = -999f;
        [SerializeField] private float invulnerabilityDuration = 0.5f; // Неуязвимость полсекунды

        public float CurrentHealth => currentHealth;
        public float MaxHealth => _stats.GetStat(StatType.HealthMax);
        public bool IsDead => currentHealth <= 0;

        private void Awake()
        {
            _stats = GetComponent<PlayerStats>();
        }

        private void Start()
        {
            _lastMaxHealth = MaxHealth;
            currentHealth = MaxHealth;
            NotifyHealthChanged();
        }

        private void OnEnable()
        {
            if (_stats != null) _stats.OnStatChanged += HandleStatChanged;
        }

        private void OnDisable()
        {
            if (_stats != null) _stats.OnStatChanged -= HandleStatChanged;
        }

        private void HandleStatChanged(StatType stat, float newValue)
        {
            if (stat == StatType.HealthMax)
            {
                float delta = newValue - _lastMaxHealth;
                if (delta > 0) currentHealth += delta;
                _lastMaxHealth = newValue;
                currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
                NotifyHealthChanged();
            }
        }

        public void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0) return;
            if (Time.time < _lastDamageTime + invulnerabilityDuration) return; // <-- ИГНОРИРУЕМ УРОН

            _lastDamageTime = Time.time;
            currentHealth = Mathf.Max(currentHealth - amount, 0);
            NotifyHealthChanged();  
            OnDamaged?.Invoke(); // <-- ДОБАВИТЬ ВЫЗОВ СОБЫТИЯ
            if (IsDead) OnDeath?.Invoke();
        }

        public void Heal(float amount)
        {
            if (IsDead || amount <= 0) return;
            currentHealth = Mathf.Min(currentHealth + amount, MaxHealth);
            NotifyHealthChanged();
        }

        private void NotifyHealthChanged() => OnHealthChanged?.Invoke(currentHealth, MaxHealth);

        // ==========================================
        // ISaveable
        // ==========================================
        public string SaveKey => "PlayerHealth";

        public string CaptureState()
        {
            return JsonUtility.ToJson(new HealthData { Health = currentHealth });
        }

        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<HealthData>(json);
            if (data != null)
            {
                _lastMaxHealth = MaxHealth; // Обновляем перед восстановлением
                currentHealth = Mathf.Clamp(data.Health, 0f, MaxHealth);
                NotifyHealthChanged();
            }
        }

        [Serializable]
        private class HealthData { public float Health; }
    }
}