using System;
using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerMana : MonoBehaviour, ISaveable
    {
        [Header("Текущее состояние")]
        [SerializeField] private float currentMana;

        [Header("Настройки восстановления")]
        [Tooltip("Сколько маны восстанавливается в секунду")]
        [SerializeField] private float regenRate = 2f;

        private PlayerStats _stats;
        private float _lastMaxMana;

        public event Action<float, float> OnManaChanged; // (current, max)

        public float CurrentMana => currentMana;
        public float MaxMana => _stats.GetStat(StatType.ManaMax);

        private void Awake()
        {
            _stats = GetComponent<PlayerStats>();
        }

        private void Start()
        {
            _lastMaxMana = MaxMana;
            currentMana = MaxMana;
            NotifyManaChanged();
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
            // Плавная регенерация маны
            if (currentMana < MaxMana)
            {
                currentMana += regenRate * Time.deltaTime;
                currentMana = Mathf.Min(currentMana, MaxMana);
                NotifyManaChanged();
            }
        }

        private void HandleStatChanged(StatType stat, float newValue)
        {
            if (stat == StatType.ManaMax)
            {
                float delta = newValue - _lastMaxMana;
                if (delta > 0) currentMana += delta; // При повышении уровня/навыка добавляем ману

                _lastMaxMana = newValue;
                currentMana = Mathf.Clamp(currentMana, 0, MaxMana);
                NotifyManaChanged();
            }
        }

        /// <summary>
        /// Тратит ману (для заклинаний). Возвращает true, если хватило.
        /// </summary>
        public bool TryConsume(float amount)
        {
            if (amount <= 0) return true;
            if (currentMana < amount) return false;

            currentMana -= amount;
            NotifyManaChanged();
            return true;
        }

        public void RestoreMana(float amount)
        {
            if (amount <= 0) return;
            currentMana = Mathf.Min(currentMana + amount, MaxMana);
            NotifyManaChanged();
        }

        private void NotifyManaChanged()
        {
            OnManaChanged?.Invoke(currentMana, MaxMana);
        }

        // ==========================================
        // ISaveable
        // ==========================================
        public string SaveKey => "PlayerMana";

        public string CaptureState()
        {
            return JsonUtility.ToJson(new ManaData { Mana = currentMana });
        }

        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<ManaData>(json);
            if (data != null)
            {
                _lastMaxMana = MaxMana;
                currentMana = Mathf.Clamp(data.Mana, 0f, MaxMana);
                NotifyManaChanged();
            }
        }

        [Serializable]
        private class ManaData { public float Mana; }
    }
}