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

            // 🛠 ФИКС: Прячем луч, если открыт любой UI
            if ((TradeUI.Instance != null && TradeUI.Instance.IsOpen) ||
                (InventoryUI.Instance != null && InventoryUI.Instance.IsOpen) ||
                (ChestUI.Instance != null && ChestUI.Instance.IsOpen) ||
                DialogueRunner.IsDialogueActive)
            {
                ClearCurrentInteractable();
                return;
            }

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer);

            if (hitSomething && hit.collider.TryGetComponent(out IInteractable interactable))
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
                return;
            }

            ClearCurrentInteractable();
        }

        private void OnInteractPressed(InputAction.CallbackContext ctx)
        {
            // 🛠 ФИКС: Не взаимодействуем, если открыт UI
            if ((TradeUI.Instance != null && TradeUI.Instance.IsOpen) ||
                (InventoryUI.Instance != null && InventoryUI.Instance.IsOpen) ||
                (ChestUI.Instance != null && ChestUI.Instance.IsOpen) ||
                DialogueRunner.IsDialogueActive)
                return;

            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(gameObject);
                ClearCurrentInteractable();
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