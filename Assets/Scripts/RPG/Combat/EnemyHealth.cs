using UnityEngine;

namespace Game.RPG
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [Header("Data")]
        [SerializeField] private EnemyDataSO data;

        private float _currentHealth;

        public event System.Action<float, float> OnHealthChanged;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => data != null ? data.MaxHealth : 50f;
        public bool IsDead => _currentHealth <= 0;


        private void Awake()
        {
            if(data != null)
                _currentHealth = data.MaxHealth;
            else
                _currentHealth = 50f;
        }       

        public void TakeDamage(float amount, GameObject source = null)
        {
            if (IsDead) return;

            _currentHealth -= amount;

            OnHealthChanged?.Invoke(_currentHealth, MaxHealth); // <-- ДОБАВИТЬ
            Debug.Log($"[Враг] {gameObject.name} получил {amount} урона. Осталось HP: {_currentHealth}/{MaxHealth}");

            // Обновляем боевой режим у игрока, когда мы получаем урон
            if (source != null && source.TryGetComponent(out CombatState combatState))
            {
                combatState.RefreshCombat();
            }

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"[Враг] {gameObject.name} погиб!");

            // 1. Начисляем опыт
            if (data != null)
            {
                var playerLeveling = FindFirstObjectByType<PlayerLeveling>();
                if (playerLeveling != null) playerLeveling.AddXp(data.XpReward);

                // 🛠 НОВОЕ: Сообщаем квестовой системе об убийстве
                if (QuestManager.Instance != null)
                {
                    QuestManager.Instance.ReportKill(data.EnemyId);
                }
            }


            //// 2. Выводим из боевого режима
            //// Обновляем боевой режим у игрока, когда мы получаем урон
            //var combatState = FindFirstObjectByType<CombatState>();
            //if (combatState != null) combatState.RefreshCombat();

            // 3. Отключаем поведение и физику (превращаем в труп)
            this.enabled = false; // Отключаем получение урона
            if (TryGetComponent(out UnityEngine.AI.NavMeshAgent agent)) agent.enabled = false;
            if (TryGetComponent(out Collider col)) col.enabled = false;

            // Опционально: поворачиваем модель, чтобы она упала (если нет анимации смерти)
            transform.Rotate(Vector3.right, 90f);

            // 4. В будущем здесь мы повесим скрипт для лута (обыск трупа)
            Debug.Log("[Враг] Превратился в труп. Можно добавить логику обыска.");

            // (УДАЛИЛИ Destroy(gameObject, 2f) - теперь труп остается навсегда)
        }
    }
}
