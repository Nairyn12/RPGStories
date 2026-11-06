using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float interactDistance = 15f;
        [SerializeField] private LayerMask interactLayer; // Слой, на котором лежат предметы
        [SerializeField] private float rayStartOffset = 1f;

        //[Header("UI Events")]
        public event Action<string> OnInteractPromptChanged; // Событие для UI

        private PlayerControls _controls;
        private IInteractable _currentInteractable;

        private void Awake()
        {
            _controls = new PlayerControls();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.Interact.performed += OnInteractPressed;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.Interact.performed -= OnInteractPressed;
        }

        private void Update()
        {
            if (cameraTransform == null) return;

            // Луч всегда летит ИЗ КАМЕРЫ (она двигается вместе с игроком)
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

            // Используем interactLayer. Благодаря этому луч пролетит СКВОЗЬ коллайдер игрока,
            // если слой Player не выбран в этой маске.
            bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer);

            Color rayColor = hitSomething ? Color.green : Color.red;
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactDistance, rayColor);

            if (hitSomething && hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (_currentInteractable != interactable)
                {
                    _currentInteractable = interactable;
                    OnInteractPromptChanged?.Invoke(interactable.GetInteractionText());
                }
                return;
            }

            if (_currentInteractable != null)
            {
                _currentInteractable = null;
                OnInteractPromptChanged?.Invoke(null);
            }
        }

        private void OnInteractPressed(InputAction.CallbackContext ctx)
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(gameObject);
            }
        }
    }
}
