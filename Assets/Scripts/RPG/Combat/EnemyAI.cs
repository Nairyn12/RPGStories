using UnityEngine;
using UnityEngine.AI;

namespace Game.RPG
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyAI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private EnemyDataSO data;

        private enum State { Idle, Chasing, Attacking, Dead }
        private State _currentState = State.Idle;

        private NavMeshAgent _agent;
        private EnemyHealth _health;
        private Transform _playerTransform;
        private PlayerHealth _playerHealth;
        private CombatState _playerCombatState;

        private float _lastAttackTime;
        private Vector3 _startPosition;
        private float _wanderRadius = 5f; // Радиус патрулирования

        [SerializeField] private GameObject healthBarPrefab;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<EnemyHealth>();
            _startPosition = transform.position;

            if (data != null)
            {
                _agent.speed = data.MovementSpeed;
                _agent.stoppingDistance = data.AttackRange - 0.5f;
            }

            // 1. Поиск игрока
            var playerStats = FindFirstObjectByType<PlayerStats>();
            if (playerStats != null) _playerTransform = playerStats.transform;
            _playerHealth = FindFirstObjectByType<PlayerHealth>();
            _playerCombatState = FindFirstObjectByType<CombatState>();

            // 2. Спавн полоски здоровья (то, что мы случайно удалили)
            if (healthBarPrefab != null)
            {
                Canvas canvas = FindFirstObjectByType<Canvas>();
                if (canvas != null)
                {
                    GameObject barObj = Instantiate(healthBarPrefab, canvas.transform);
                    if (barObj.TryGetComponent(out EnemyHealthBar bar))
                    {
                        bar.Initialize(_health, transform);
                    }
                }
            }
        }

        private void Update()
        {
            if (_currentState == State.Dead || _health.IsDead) return;
            if (_playerTransform == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            switch (_currentState)
            {
                case State.Idle:
                    // Если игрок в радиусе обнаружения, начинаем погоню
                    if (distanceToPlayer <= data.DetectionRadius && CanSeePlayer())
                    {
                        _currentState = State.Chasing;
                        if (_playerCombatState != null) _playerCombatState.EnterCombat();
                        Debug.Log($"[ИИ] {gameObject.name} заметил игрока!");
                    }
                    break;

                case State.Chasing:
                    // Если игрок убежал слишком далеко, возвращаемся в Idle
                    if (distanceToPlayer > data.DetectionRadius * 1.5f)
                    {
                        _currentState = State.Idle;
                        _agent.SetDestination(_startPosition);
                        break;
                    }

                    // Бежим к игроку
                    _agent.SetDestination(_playerTransform.position);

                    // Если подошли на дистанцию удара — атакуем
                    if (distanceToPlayer <= data.AttackRange)
                    {
                        _currentState = State.Attacking;
                    }

                    if (_playerCombatState != null) _playerCombatState.RefreshCombat();
                    break;

                case State.Attacking:
                    // Стоим на месте и смотрим на игрока
                    _agent.SetDestination(transform.position);
                    Vector3 lookPos = _playerTransform.position - transform.position;
                    lookPos.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), Time.deltaTime * 5f);

                    // Если игрок отошел, снова бежим за ним
                    if (distanceToPlayer > data.AttackRange)
                    {
                        _currentState = State.Chasing;
                        break;
                    }

                    // Бьем с кулдауном
                    if (Time.time >= _lastAttackTime + data.AttackCooldown)
                    {
                        PerformAttack();
                        _lastAttackTime = Time.time;
                    }

                    if (_playerCombatState != null) _playerCombatState.RefreshCombat();
                    break;
            }
        }

        private bool CanSeePlayer()
        {
            Vector3 dirToPlayer = (_playerTransform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            // Если игрок в пределах угла обзора (например, 120 градусов, значит +/- 60 от центра)
            if (angle < data.FieldOfView / 2f)
            {
                // Можно добавить Raycast, чтобы враг не видел сквозь стены, но для теста пока хватит угла
                return true;
            }
            return false;
        }

        private void PerformAttack()
        {
            Debug.Log($"[ИИ] {gameObject.name} наносит удар!");

            // Убрали проверку дистанции, так как State.Attacking уже проверяет её
            if (_playerHealth != null)
            {
                _playerHealth.TakeDamage(data.Damage);
            }
        }
    }
}