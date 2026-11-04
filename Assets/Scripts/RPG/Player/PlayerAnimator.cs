using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerController controller; // Для проверки земли
        [SerializeField] private PlayerCombat combat;

        private Animator _animator;

        // Хеши параметров (чтобы не тратить память на строки каждый кадр)
        private int _speedHash;
        private int _isGroundedHash;
        private int _verticalVelHash;
        private int _isCrouchHash;
        private int _hitHash;
        private int _moveZHash;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _speedHash = Animator.StringToHash("Speed");
            _isGroundedHash = Animator.StringToHash("IsGrounded");
            _verticalVelHash = Animator.StringToHash("VerticalVelocity");
            _isCrouchHash = Animator.StringToHash("IsCrouching");
            _hitHash = Animator.StringToHash("Hit");
            _moveZHash = Animator.StringToHash("MoveZ");
        }

        private void Update()
        {
            if (rb == null || controller == null) return;

            // 1. Скорость для Idle/Walk/Run
            Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            _animator.SetFloat(_speedHash, horizontalVel.magnitude, 0.1f, Time.deltaTime);

            // 2. Локальная скорость (вперед/назад)
            // Переводим мировую скорость в локальные координаты персонажа
            Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
            // localVel.z > 0 (вперед), localVel.z < 0 (назад)
            _animator.SetFloat(_moveZHash, localVel.z, 0.1f, Time.deltaTime);

            // 3. Вертикальная скорость и земля
            _animator.SetFloat(_verticalVelHash, rb.linearVelocity.y);
            _animator.SetBool(_isGroundedHash, controller.IsGrounded);
        }


        // Этот метод будет вызывать наш PlayerCombat при атаке
        public void PlayAttackAnimation()
        {
            _animator.SetTrigger("Attack");
        }

        public void PlayHitAnimation()
        {
            _animator.SetTrigger(_hitHash);
        }
    }
}