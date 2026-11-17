using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerCombat : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerStamina stamina;
        [SerializeField] private CombatState combatState;
        [SerializeField] private PlayerController playerController; // <-- ДОБАВИТЬ
        [SerializeField] private PlayerAnimator playerAnimator;     // <-- ДОБАВИТЬ
        [SerializeField] private WeaponHitbox hitbox;
        [SerializeField] private WeaponItemSO currentWeapon;

        // Свойство, которое читает PlayerController
        public bool IsAttacking { get; private set; }


        private PlayerControls _controls;
        private float _lastAttackTime;

        private float _attackDuration = 0.867f; // Длительность атаки (должна совпадать с анимацией)

        private void Awake()
        {
            _controls = new PlayerControls();

            // АВТОПОИСК
            if (stats == null) stats = FindFirstObjectByType<PlayerStats>();
            if (stamina == null) stamina = FindFirstObjectByType<PlayerStamina>();
            if (combatState == null) combatState = FindFirstObjectByType<CombatState>();
            if (playerController == null) playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.Attack.performed += OnAttackPerformed;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.Attack.performed -= OnAttackPerformed;
        }

        private void OnAttackPerformed(InputAction.CallbackContext ctx)
        {
            TryAttack();
        }

        private void TryAttack()
        {
            // Если идет диалог - блокируем атаку
            if (DialogueRunner.IsDialogueActive) return;

            // 1. Если игра на паузе (открыт инвентарь) - не атакуем
            if (Time.timeScale == 0f) return;

            // 2. Если курсор мыши над UI (инвентарем, кнопками) - не атакуем
            if (UnityEngine.EventSystems.EventSystem.current != null &&
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            // 1. Проверяем, не в воздухе ли мы (блокировка атаки в прыжке)
            if (!playerController.IsGrounded) return;

            // 2. Проверяем, не атакуем ли мы уже
            if (IsAttacking) return;

            if (currentWeapon == null || hitbox == null) return;

            // 3. Кулдаун
            if (Time.time < _lastAttackTime + currentWeapon.AttackCooldown) return;

            // 4. Стамина (если атака требует)
            if (currentWeapon.StaminaCost > 0)
            {
                if (!stamina.TryConsume(currentWeapon.StaminaCost)) return;
            }

            // 5. Входим в боевой режим
            if (combatState != null) combatState.EnterCombat();

            // 6. Включаем анимацию и хитбокс
            IsAttacking = true;
            if (playerAnimator != null) playerAnimator.PlayAttackAnimation();

            float damage = currentWeapon.CalculateDamage(stats);
            hitbox.Activate(gameObject, damage, 0.3f);
           
            _lastAttackTime = Time.time;

            Debug.Log($"[Combat] Атака! Урон: {damage:F1}.");

            // 7. Запускаем таймер окончания атаки
            StartCoroutine(AttackRoutine());
        }

        public void EquipWeapon(WeaponItemSO newWeapon)
        {
            currentWeapon = newWeapon;
            Debug.Log($"[Combat] Экипировано оружие: {(newWeapon != null ? newWeapon.ItemName : "Кулаки")}");
        }

        private IEnumerator AttackRoutine()
        {
            yield return new WaitForSeconds(_attackDuration);
            IsAttacking = false;
        }
    }
}
