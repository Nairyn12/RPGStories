using System;
using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerStamina : MonoBehaviour, ISaveable
    {
        [Header("Текущее состояние")]
        [SerializeField] private float currentStamina;

        [Header("Настройки восстановления")]
        [Tooltip("Сколько стамины восстанавливается в секунду")]
        [SerializeField] private float regenRate = 15f;

        [Tooltip("Задержка перед началом восстановления после траты (в секундах)")]
        [SerializeField] private float regenDelay = 1f;

        private PlayerStats _stats;
        private float _lastMaxStamina;
        private float _lastConsumeTime = -999f;

        public event Action<float, float> OnStaminaChanged; // (current, max)

        public float CurrentStamina => currentStamina;
        public float MaxStamina => _stats.GetStat(StatType.StaminaMax);

        // Процент стамины (от 0 до 1) - пригодится для UI полоски
        public float StaminaPercent => MaxStamina > 0 ? currentStamina / MaxStamina : 0f;

        // Флаг, чтобы контроллер знал, можно ли сейчас бежать
        public bool HasEnoughStamina => currentStamina > 0f;

        private void Awake()
        {
            _stats = GetComponent<PlayerStats>();
        }

        private void Start()
        {
            _lastMaxStamina = MaxStamina;
            currentStamina = MaxStamina; // На старте игры стамина полная
            NotifyStaminaChanged();
        }

        private void OnEnable()
        {
            if (_stats != null) _stats.OnStatChanged += HandleStatChanged;
        }

        private void OnDisable()
        {
            if (_stats != null) _stats.OnStatChanged -= HandleStatChanged;
        }

        private void Update()
        {
            // Автоматическое восстановление стамины, если прошло достаточно времени
            if (Time.time >= _lastConsumeTime + regenDelay && currentStamina < MaxStamina)
            {
                currentStamina += regenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, MaxStamina);
                NotifyStaminaChanged();
            }
        }

        private void HandleStatChanged(StatType stat, float newValue)
        {
            if (stat == StatType.StaminaMax)
            {
                // Если максимальная стамина выросла (например, повысили уровень),
                // добавляем разницу к текущей, чтобы игрок не терял прогресс.
                float delta = newValue - _lastMaxStamina;
                if (delta > 0) currentStamina += delta;

                _lastMaxStamina = newValue;
                currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);
                NotifyStaminaChanged();
            }
        }

        /// <summary>
        /// Пытается потратить стамину. Возвращает true, если хватило.
        /// </summary>
        public bool TryConsume(float amount)
        {
            if (amount <= 0) return true;
            if (currentStamina < amount) return false;

            currentStamina -= amount;
            _lastConsumeTime = Time.time; // Сбрасываем таймер задержки регена
            NotifyStaminaChanged();
            return true;
        }

        /// <summary>
        /// Принудительно восстанавливает стамину (например, зельем).
        /// </summary>
        public void RestoreStamina(float amount)
        {
            if (amount <= 0) return;
            currentStamina = Mathf.Min(currentStamina + amount, MaxStamina);
            NotifyStaminaChanged();
        }

        private void NotifyStaminaChanged()
        {
            OnStaminaChanged?.Invoke(currentStamina, MaxStamina);
        }

        // ==========================================
        // ISaveable
        // ==========================================
        public string SaveKey => "PlayerStamina";

        public string CaptureState()
        {
            return JsonUtility.ToJson(new StaminaData { Stamina = currentStamina });
        }

        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<StaminaData>(json);
            if (data != null)
            {
                _lastMaxStamina = MaxStamina;
                currentStamina = Mathf.Clamp(data.Stamina, 0f, MaxStamina);
                NotifyStaminaChanged();
            }
        }

        [Serializable]
        private class StaminaData { public float Stamina; }
    }
}