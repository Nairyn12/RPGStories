using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactLayer;

        public event Action<string> OnInteractPromptChanged;

        private PlayerControls _controls;
        private IInteractable _currentInteractable;

        private void Awake() => _controls = new PlayerControls();

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

            if ((TradeUI.Instance != null && TradeUI.Instance.IsOpen) ||
                (InventoryUI.Instance != null && InventoryUI.Instance.IsOpen) ||
                (ChestUI.Instance != null && ChestUI.Instance.IsOpen) ||
                (LockpickingManager.Instance != null && LockpickingManager.Instance.IsActive) ||
                DialogueRunner.IsDialogueActive)
            {
                ClearCurrentInteractable();
                return;
            }

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer);

            if (hitSomething)
            {
                // 🛠 Используем GetComponentInParent, чтобы находить скрипты на корневых объектах
                var lockedObj = hit.collider.GetComponentInParent<LockedObject>();
                if (lockedObj != null)
                {
                    HandleInteractable(lockedObj);
                    return;
                }

                var interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    HandleInteractable(interactable);
                    return;
                }
            }

            ClearCurrentInteractable();
        }

        private void HandleInteractable(IInteractable interactable)
        {
            if (interactable is UnityEngine.Object obj && obj == null)
            {
                ClearCurrentInteractable();
                return;
            }

            if (_currentInteractable != interactable)
            {
                _currentInteractable = interactable;
                OnInteractPromptChanged?.Invoke(interactable.GetInteractionText());
            }
        }

        private void OnInteractPressed(InputAction.CallbackContext ctx)
        {
            if ((TradeUI.Instance != null && TradeUI.Instance.IsOpen) ||
                (InventoryUI.Instance != null && InventoryUI.Instance.IsOpen) ||
                (ChestUI.Instance != null && ChestUI.Instance.IsOpen) ||
                (LockpickingManager.Instance != null && LockpickingManager.Instance.IsActive) ||
                DialogueRunner.IsDialogueActive)
                return;

            if (_currentInteractable != null)
            {
                Debug.Log($"[Interactor] Нажата E, вызов Interact() для {(_currentInteractable as MonoBehaviour).gameObject.name}");
                _currentInteractable.Interact(gameObject);
                ClearCurrentInteractable();
            }
            else
            {
                Debug.Log("[Interactor] Нажата E, но цель не найдена (луч не видит объект).");
            }
        }

        private void ClearCurrentInteractable()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable = null;
                OnInteractPromptChanged?.Invoke(null);
            }
        }
    }
}