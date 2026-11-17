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

            // АВТОПОИСК
            if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
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
            // 🛠 ФИКС ПОДСКАЗКИ: Если идет диалог - прячем подсказку и выходим
            if (DialogueRunner.IsDialogueActive)
            {
                if (_currentInteractable != null)
                {
                    _currentInteractable = null;
                    OnInteractPromptChanged?.Invoke(null); // Очищаем текст подсказки
                }
                return; // Не пускаем луч, пока идет диалог
            }

            if (cameraTransform == null) return;

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer);

            // Если мы попали в объект и там есть IInteractable
            if (hitSomething && hit.collider.TryGetComponent(out IInteractable interactable))
            {
                // Проверка на "фейковый нулл": если объект уничтожен, Unity вернет true для этого теста
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
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(gameObject);
                ClearCurrentInteractable(); // Мгновенно очищаем после подбора
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
