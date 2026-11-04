using System.Collections.Generic;
using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(Collider))]
    public class WeaponHitbox : MonoBehaviour
    {
        [Tooltip("Слой, по которому может бить оружие (например, Enemy)")]
        public LayerMask targetLayer;

        private Collider _collider;
        private float _damage;
        private GameObject _owner;
        private bool _isActive;

        // Чтобы не ударить одного и того же врага дважды за один взмах
        private HashSet<IDamageable> _hitTargets = new HashSet<IDamageable>();

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            _collider.enabled = false; // Выключаем по умолчанию
        }

        public void Activate(GameObject owner, float damage, float duration)
        {
            _owner = owner;
            _damage = damage;
            _isActive = true;
            _hitTargets.Clear();

            _collider.enabled = true;

            // Выключаем хитбокс через заданное время
            Invoke(nameof(Deactivate), duration);
        }

        private void Deactivate()
        {
            _isActive = false;
            _collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive) return;
            if (other.gameObject == _owner) return; // Не бьем себя

            // Проверяем, есть ли у объекта интерфейс IDamageable (из Шага 1)
            if (other.TryGetComponent(out IDamageable damageable))
            {
                if (!_hitTargets.Contains(damageable))
                {
                    _hitTargets.Add(damageable);
                    damageable.TakeDamage(_damage, _owner);
                    Debug.Log($"[Hitbox] Попадание по {other.name}! Урон: {_damage:F1}");
                }
            }
        }
    }
}
