using UnityEngine;
using UnityEngine.UI;

namespace Game.RPG
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        private EnemyHealth _enemyHealth;
        private Transform _target;
        private Camera _mainCamera;

        [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0); // Высота над головой

        public void Initialize(EnemyHealth health, Transform target)
        {
            _enemyHealth = health;
            _target = target;
            _mainCamera = Camera.main;

            // Подписываемся на изменения здоровья врага
            // (Нам нужно добавить событие в EnemyHealth, сделаем это чуть ниже)
            _enemyHealth.OnHealthChanged += UpdateBar;
            gameObject.SetActive(true);
            UpdateBar(_enemyHealth.CurrentHealth, _enemyHealth.MaxHealth);
        }

        private void LateUpdate()
        {
            if (_target == null || _mainCamera == null) return;

            // 1. Переводим мировую позицию врага в координаты экрана (пиксели)
            Vector3 screenPos = _mainCamera.WorldToScreenPoint(_target.position + offset);

            // 2. Если враг находится за спиной камеры, прячем полоску
            // (у WorldToScreenPoint Z становится отрицательным, если объект за камерой)
            if (screenPos.z < 0)
            {
                if (fillImage != null) fillImage.enabled = false;
                return;
            }
            else
            {
                if (fillImage != null && !_enemyHealth.IsDead) fillImage.enabled = true;
            }

            // 3. Присваиваем позицию UI-элементу
            transform.position = screenPos;

            // ВАЖНО: удали строчку transform.rotation = _mainCamera.transform.rotation; 
            // В режиме Screen Space - Overlay UI сам всегда смотрит на камеру. 
            // Вращение может сломать отображение.
        }

        private void UpdateBar(float current, float max)
        {
            if (fillImage != null) fillImage.fillAmount = current / max;
            // Выключаем ТОЛЬКО если враг реально мертв
            if (_enemyHealth != null && _enemyHealth.IsDead)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (_enemyHealth != null) _enemyHealth.OnHealthChanged -= UpdateBar;
        }
    }
}