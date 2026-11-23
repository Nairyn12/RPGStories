using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerStamina stamina;
        [SerializeField] private PlayerHealth health;
        [SerializeField] private PlayerCombat playerCombat; // <-- ДОБАВИТЬ
        [SerializeField] private CombatState combatState;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private PlayerInventory inventory;

        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float crouchSpeed = 1.5f;
        [SerializeField] private float acceleration = 15f;
        [SerializeField] private float rotationSpeed = 12f;

        [Tooltip("Насколько процентов растет скорость за 1 единицу характеристик (Speed, Acrobatics)")]
        [SerializeField] private float speedBonusPerPoint = 0.025f; // 2.5%

        [Header("Jump & Dodge Settings")]
        [SerializeField] private float baseJumpForce = 7f;
        [SerializeField] private float baseDodgeForce = 10f;
        [SerializeField] private float staminaCostPerSecondSprint = 10f;
        [SerializeField] private float staminaCostDodge = 20f;
        [SerializeField] private float jumpAnticipationTime = 0.6f; // <-- ДОБАВИТЬ
        private bool _isJumpingAnticipation = false; // <-- ДОБАВИТЬ

        [Tooltip("Множитель прыжка/уклонения за 1 единицу Акробатики (0.01 = 1%)")]
        [SerializeField] private float acrobaticsBonusPerPoint = 0.01f;

        [Header("Fall Damage Settings")]
        [SerializeField] private float baseSafeFallHeight = 2f;
        [Tooltip("Насколько метров повышается безопасная высота за 1 единицу Акробатики")]
        [SerializeField] private float acrobaticsFallResistance = 0.1f;
        [SerializeField] private float damagePerMeter = 10f;

        [Header("Crouch Settings")]
        [SerializeField] private float standHeight = 1.8f;
        [SerializeField] private float crouchHeight = 1.0f;

        [Header("Landing Settings")]
        [Tooltip("Сколько секунд игрок не может двигаться после приземления")]
        [SerializeField] private float landingLockDuration = 1f;

        public bool IsLanding { get; private set; } // Флаг для других скриптов (если нужно)

        private Rigidbody _rb;
        private PlayerControls _controls;
        private Vector2 _moveInput;
        private bool _isSprintHeld;
        private bool _isCrouching;
        private bool _isGrounded;
        private bool _isStunned = false; // Флаг стана
        public bool IsGrounded => _isGrounded; // <-- ДОБАВИТЬ ЭТУ СТРОКУ

        private CapsuleCollider _capsule;
        private float _maxFallHeight;
        private bool _wasGroundedLastFrame;

        [SerializeField] private PlayerAnimator playerAnimator;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _capsule = GetComponent<CapsuleCollider>();
            _controls = new PlayerControls();

            // АВТОПОИСК (если ссылки потерялись)
            if (stats == null) stats = FindFirstObjectByType<PlayerStats>();
            if (stamina == null) stamina = FindFirstObjectByType<PlayerStamina>();
            if (health == null) health = FindFirstObjectByType<PlayerHealth>();
            if (playerCombat == null) playerCombat = FindFirstObjectByType<PlayerCombat>();
            if (combatState == null) combatState = FindFirstObjectByType<CombatState>();
            if (inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();
        }

        /// ==========================================
        // ОБРАБОТЧИКИ ХОТБАРА
        // ==========================================

        private void HandleSlot1(InputAction.CallbackContext ctx) => UseHotbar(0);
        private void HandleSlot2(InputAction.CallbackContext ctx) => UseHotbar(1);
        private void HandleSlot3(InputAction.CallbackContext ctx) => UseHotbar(2);
        private void HandleSlot4(InputAction.CallbackContext ctx) => UseHotbar(3);
        private void HandleSlot5(InputAction.CallbackContext ctx) => UseHotbar(4);
        private void HandleSlot6(InputAction.CallbackContext ctx) => UseHotbar(5);
        private void HandleSlot7(InputAction.CallbackContext ctx) => UseHotbar(6);
        private void HandleSlot8(InputAction.CallbackContext ctx) => UseHotbar(7);

        private void UseHotbar(int index)
        {
            // 1. Если мышка наведена на слот в инвентаре - назначаем предмет в хотбар
            if (InventorySlotUI.CurrentlyHoveredSlot != null)
            {
                if (InventorySlotUI.CurrentlyHoveredSlot.Slot != null && InventorySlotUI.CurrentlyHoveredSlot.Slot.Item != null)
                {
                    // 🛠 НОВАЯ ПРОВЕРКА
                    if (!InventorySlotUI.CurrentlyHoveredSlot.Slot.Item.CanBeInHotbar)
                    {
                        Debug.LogWarning("[Hotbar] Этот предмет нельзя назначить на быструю клавишу!");
                        return;
                    }

                    inventory.SetQuickSlot(index, InventorySlotUI.CurrentlyHoveredSlot.Slot);
                    return;
                }
            }

            // 2. Если инвентарь закрыт - используем предмет из хотбара
            if (Time.timeScale > 0f)
            {
                inventory.UseQuickSlot(index);
            }
        }

        private void OnEnable()
        {
            // Включаем основное управление
            _controls.Player.Enable();

            // Включаем управление хотбаром
            _controls.Hotbar.Enable();

            _controls.Player.Sprint.started += ctx => _isSprintHeld = true;
            _controls.Player.Sprint.canceled += ctx => _isSprintHeld = false;

            _controls.Player.Crouch.performed += ctx => ToggleCrouch();
            _controls.Player.JumpOrDodge.performed += ctx => HandleJumpOrDodge();

            // Подписываемся на слоты хотбара (1-8)
            _controls.Hotbar.Slot1.performed += HandleSlot1;
            _controls.Hotbar.Slot2.performed += HandleSlot2;
            _controls.Hotbar.Slot3.performed += HandleSlot3;
            _controls.Hotbar.Slot4.performed += HandleSlot4;
            _controls.Hotbar.Slot5.performed += HandleSlot5;
            _controls.Hotbar.Slot6.performed += HandleSlot6;
            _controls.Hotbar.Slot7.performed += HandleSlot7;
            _controls.Hotbar.Slot8.performed += HandleSlot8;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Hotbar.Disable(); // Выключаем карту

            // Отписываемся от событий
            _controls.Hotbar.Slot1.performed -= HandleSlot1;
            _controls.Hotbar.Slot2.performed -= HandleSlot2;
            _controls.Hotbar.Slot3.performed -= HandleSlot3;
            _controls.Hotbar.Slot4.performed -= HandleSlot4;
            _controls.Hotbar.Slot5.performed -= HandleSlot5;
            _controls.Hotbar.Slot6.performed -= HandleSlot6;
            _controls.Hotbar.Slot7.performed -= HandleSlot7;
            _controls.Hotbar.Slot8.performed -= HandleSlot8;
        }

        private void Start()
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            _maxFallHeight = transform.position.y;

            if (health != null) health.OnDamaged += HandleDamageStun;
        }

        private void Update()
        {
            _moveInput = _controls.Player.Move.ReadValue<Vector2>();
            CheckGround();
            HandleFallTracking();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleRotation();
            HandleSprintStamina();
        }

        // ==== ЛОГИКА ДВИЖЕНИЯ ====

        private void HandleMovement()
        {
            // Блокировка во время атаки ИЛИ во время приземления
            if ((playerCombat != null && playerCombat.IsAttacking) || IsLanding || _isStunned)
            {
                // Гасим горизонтальную скорость
                _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
                return;
            }                      

            // Вычисляем направление относительно камеры
            Vector3 camForward = cameraTransform ? cameraTransform.forward : Vector3.forward;
            Vector3 camRight = cameraTransform ? cameraTransform.right : Vector3.right;
            camForward.y = 0f; camRight.y = 0f;
            camForward.Normalize(); camRight.Normalize();

            Vector3 desiredDir = (camForward * _moveInput.y + camRight * _moveInput.x);
            if (desiredDir.sqrMagnitude > 1f) desiredDir.Normalize();

            float statSpeedMultiplier = 1f + (stats.GetStat(StatType.Speed) * speedBonusPerPoint);

            float targetSpeed = 0f;
            if (desiredDir.sqrMagnitude > 0.01f)
            {
                if (_isSprintHeld && stamina.CanSprint) // <-- ИЗМЕНЕНО
                {
                    if (_isCrouching) ToggleCrouch();
                    targetSpeed = runSpeed;
                }
                else if (_isCrouching) targetSpeed = crouchSpeed;
                else targetSpeed = walkSpeed;

                targetSpeed *= statSpeedMultiplier;
            }

            Vector3 targetVelocity = desiredDir * targetSpeed;
            Vector3 currentVel = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            Vector3 newVel = Vector3.Lerp(currentVel, targetVelocity, acceleration * Time.fixedDeltaTime);

            _rb.linearVelocity = new Vector3(newVel.x, _rb.linearVelocity.y, newVel.z);
        }


        private void HandleRotation()
        {
            // Вычисляем направление, куда игрок хочет двигаться (относительно камеры)
            Vector3 camForward = cameraTransform ? cameraTransform.forward : Vector3.forward;
            Vector3 camRight = cameraTransform ? cameraTransform.right : Vector3.right;
            camForward.y = 0f; camRight.y = 0f;
            camForward.Normalize(); camRight.Normalize();

            Vector3 desiredDir = (camForward * _moveInput.y + camRight * _moveInput.x);

            if (combatState != null && combatState.IsInCombat)
            {
                // В БОЮ:
                // 1. Если мы жмем S (назад) или вообще не двигаемся - смотрим на камеру (на врага)
                if (_moveInput.y < -0.1f && Mathf.Abs(_moveInput.x) < 0.1f || _moveInput.sqrMagnitude < 0.01f)
                {
                    Vector3 combatForward = cameraTransform.forward;
                    combatForward.y = 0f;
                    if (combatForward.sqrMagnitude > 0.01f)
                    {
                        Quaternion targetRot = Quaternion.LookRotation(combatForward.normalized, Vector3.up);
                        _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
                    }
                }
                // 2. Если мы жмем W, A или D (вперед или вбок) - разворачиваемся туда, куда идем
                else if (desiredDir.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(desiredDir.normalized, Vector3.up);
                    _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
                }
            }
            else
            {
                // ВНЕ БОЯ: Стандартное поведение (смотрим в сторону движения)
                Vector3 horizontalVel = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
                if (horizontalVel.sqrMagnitude < 0.05f) return;

                Quaternion targetRotNormal = Quaternion.LookRotation(horizontalVel.normalized, Vector3.up);
                _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotNormal, rotationSpeed * Time.fixedDeltaTime));
            }
        }

        private void HandleSprintStamina()
        {
            bool isMoving = _moveInput.sqrMagnitude > 0.01f;
            if (_isSprintHeld && isMoving && !_isCrouching && _isGrounded)
            {
                // Тратим стамину каждый FixedUpdate (умножаем на deltaTime для независимости от FPS)
                stamina.TryConsume(staminaCostPerSecondSprint * Time.fixedDeltaTime);
            }
        }

        // ==== ПРЫЖОК И КУВЫРОК ====

        private void HandleJumpOrDodge()
        {     

            // Блокируем прыжок и кувырок, если сидим
            if (_isCrouching) return;

            if ((playerCombat != null && playerCombat.IsAttacking) || IsLanding || _isStunned) return;

            if (!_isGrounded || _isJumpingAnticipation) return;

            float acroMult = 1f + (stats.GetStat(StatType.Acrobatics) * acrobaticsBonusPerPoint);

            // Кувырок (Dodge) оставляем мгновенным, чтобы был отзывчивым
            if (combatState != null && combatState.IsInCombat)
            {
                if (stamina.TryConsume(staminaCostDodge))
                {
                    Vector3 dodgeDir = GetInputDirection();
                    _rb.AddForce(dodgeDir * baseDodgeForce * acroMult, ForceMode.VelocityChange);
                }
            }
            else
            {
                // Обычный прыжок с задержкой
                StartCoroutine(JumpAnticipationRoutine(acroMult));
            }
        }

        // НОВАЯ КОРУТИНА
        private System.Collections.IEnumerator JumpAnticipationRoutine(float acroMult)
        {
            _isJumpingAnticipation = true;

            // 1. Мгновенно запускаем анимацию (пусть Animator играет присед)
            //if (playerAnimator != null) playerAnimator.PlayJumpAnimation();

            // 2. Ждем 0.3 секунды (время замаха/приседа)
            yield return new WaitForSeconds(jumpAnticipationTime);

            // 3. Если за это время мы не упали с обрыва, то подкидываем физику
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * baseJumpForce * acroMult, ForceMode.VelocityChange);
            }

            _isJumpingAnticipation = false;
        }

        private Vector3 GetInputDirection()
        {
            Vector3 camForward = cameraTransform ? cameraTransform.forward : Vector3.forward;
            Vector3 camRight = cameraTransform ? cameraTransform.right : Vector3.right;
            camForward.y = 0f; camRight.y = 0f;

            Vector3 dir = (camForward * _moveInput.y + camRight * _moveInput.x);
            if (dir.sqrMagnitude < 0.01f) return transform.forward; // Если не жмем WASD, прыгаем/кувыркаемся вперед
            return dir.normalized;
        }

        // ==== ФИЗИКА И ПАДЕНИЕ ====

        private void CheckGround()
        {
            // Используем SphereCast для надежной проверки земли на Rigidbody (чтобы не застревать на краях)
            _isGrounded = Physics.CheckSphere(groundCheckPoint.position, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);
        }

        private void HandleFallTracking()
        {
            // Отслеживаем максимальную высоту
            if (!_isGrounded)
            {
                if (transform.position.y > _maxFallHeight)
                    _maxFallHeight = transform.position.y;
            }
            else
            {
                // Мы только что приземлились
                if (!_wasGroundedLastFrame)
                {
                    // Запускаем блокировку движения
                    StartCoroutine(LandingRoutine());

                    float fallDistance = _maxFallHeight - transform.position.y;

                    // Считаем безопасную высоту с учетом Акробатики
                    float currentSafeHeight = baseSafeFallHeight + (stats.GetStat(StatType.Acrobatics) * acrobaticsFallResistance);

                    if (fallDistance > currentSafeHeight)
                    {
                        float excess = fallDistance - currentSafeHeight;
                        float damage = excess * damagePerMeter;

                        if (health != null)
                        {
                            health.TakeDamage(damage);
                            Debug.Log($"[Fall] Упал с {fallDistance:F1}м. Безопасно: {currentSafeHeight:F1}м. Урон: {damage:F1}");
                        }
                    }
                    _maxFallHeight = transform.position.y; // Сброс
                }
            }
            _wasGroundedLastFrame = _isGrounded;
        }

        // НОВАЯ КОРУТИНА (добавь в конец класса)
        private System.Collections.IEnumerator LandingRoutine()
        {
            IsLanding = true;
            yield return new WaitForSeconds(landingLockDuration);
            IsLanding = false;
        }

        // ==== ПРИСЕД ====

        private void ToggleCrouch()
        {
            _isCrouching = !_isCrouching;

            // Меняем высоту коллайдера
            _capsule.height = _isCrouching ? crouchHeight : standHeight;
            _capsule.center = new Vector3(0, _capsule.height / 2f, 0);

            // (Опционально) Здесь мы будем сообщать врагам, что игрок в стелсе
            // gameObject.layer = _isCrouching ? LayerMask.NameToLayer("StealthPlayer") : LayerMask.NameToLayer("Player");
        }

        private void OnDestroy()
        {
            if (health != null) health.OnDamaged -= HandleDamageStun;
        }

        private void HandleDamageStun()
        {
            if (!_isStunned)
            {
                if (playerAnimator != null) playerAnimator.PlayHitAnimation(); // <-- ДОБАВИТЬ
                StartCoroutine(StunRoutine());
            }
        }

        private System.Collections.IEnumerator StunRoutine()
        {
            _isStunned = true;
            // Можно вызвать анимацию вздрагивания (playerAnimator.PlayHitAnimation())
            yield return new WaitForSeconds(0.4f);
            _isStunned = false;
        }
    }
}