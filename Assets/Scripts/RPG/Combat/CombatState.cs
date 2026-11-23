using UnityEngine;

namespace Game.RPG
{
    public class CombatState : MonoBehaviour
    {
        public bool IsInCombat { get; private set; }

        [Tooltip("Сколько секунд должно пройти без боевых действий, чтобы выйти из боя")]
        [SerializeField] private float combatCooldown = 5f;

        private float _lastCombatTime;

        public void EnterCombat()
        {
            if (!IsInCombat) Debug.Log("[Combat] Вход в боевой режим");
            IsInCombat = true;
            RefreshCombat();
        }

        /// <summary>
        /// Обновляет таймер боя. Вызывается врагами при погоне/атаке, а также при получении/нанесении урона.
        /// </summary>
        public void RefreshCombat()
        {
            IsInCombat = true; // На всякий случай
            _lastCombatTime = Time.time;
        }

        public void ForceExitCombat()
        {
            IsInCombat = false;
        }

        private void Update()
        {
            if (IsInCombat && Time.time - _lastCombatTime > combatCooldown)
            {
                IsInCombat = false;
                Debug.Log("[Combat] Выход из боевого режима (враги мертвы или убежали)");
            }
        }

        // Временный метод для тестов
        [ContextMenu("Toggle Combat Mode")]
        public void ToggleCombatMode()
        {
            if (IsInCombat) ForceExitCombat();
            else EnterCombat();
            Debug.Log($"[Combat] Ручное переключение: {(IsInCombat ? "ВКЛ" : "ВЫКЛ")}");
        }
    }
}