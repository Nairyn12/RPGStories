using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // <-- ВАЖНО

namespace Game.RPG
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private Transform gridParent;
        [SerializeField] private GameObject slotPrefab;

        private List<InventorySlotUI> _spawnedSlots = new();
        private bool _isOpen = false;
        private PlayerControls _controls; // <-- ДОБАВИТЬ
        private bool _isDirty;

        private void Awake()
        {
            _controls = new PlayerControls(); // <-- ДОБАВИТЬ
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.ToggleInventory.performed += OnToggleInventory; // <-- Подписываемся на новую кнопку
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.ToggleInventory.performed -= OnToggleInventory; // <-- Отписываемся
        }

        private void Start()
        {
            if (inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();

            if (inventory != null)
                inventory.OnInventoryChanged += MarkDirty;

            inventoryPanel.SetActive(false);
            _isOpen = false;
        }

        private void OnDestroy()
        {
            if (inventory != null) inventory.OnInventoryChanged -= MarkDirty;
        }

        // Вместо прямого вызова UpdateUI, мы просто ставим флаг
        // Вместо прямого вызова UpdateUI, мы просто ставим флаг
        private void MarkDirty()
        {
            if (_isOpen) _isDirty = true;
        }

        // Обработчик нажатия кнопки
        private void OnToggleInventory(InputAction.CallbackContext ctx)
        {
            // Запрещаем открывать инвентарь во время диалога
            if (DialogueRunner.IsDialogueActive) return;

            ToggleInventory();
        }

        public void ToggleInventory()
        {
            _isOpen = !_isOpen;
            inventoryPanel.SetActive(_isOpen);

            if (_isOpen)
            {
                UpdateUI();
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void UpdateUI()
        {
            if (inventory == null || gridParent == null || slotPrefab == null)
            {
                Debug.LogError("[InventoryUI] Не назначены ссылки в инспекторе! UI не может обновиться.");
                return;
            }

            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();

            foreach (var slot in _spawnedSlots)
            {
                if (slot != null) slot.gameObject.SetActive(false);
            }

            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                InventorySlotUI slotUI;
                if (i < _spawnedSlots.Count)
                {
                    slotUI = _spawnedSlots[i];
                }
                else
                {
                    GameObject slotObj = Instantiate(slotPrefab, gridParent);
                    slotUI = slotObj.GetComponent<InventorySlotUI>();
                    _spawnedSlots.Add(slotUI);
                }

                slotUI.gameObject.SetActive(true);
                slotUI.transform.SetAsLastSibling();
                slotUI.Setup(inventory.Slots[i], inventory);
            }
        }

        public void ForceUpdateUI()
        {
            inventory = FindFirstObjectByType<PlayerInventory>();
            if (inventory != null)
            {
                // Отписываемся и подписываемся заново, чтобы не было дублей
                inventory.OnInventoryChanged -= MarkDirty;
                inventory.OnInventoryChanged += MarkDirty;
            }
            UpdateUI();
            Debug.Log("[InventoryUI] Ссылки и UI обновлены.");
        }

        private void LateUpdate()
        {
            // Выполняем перестройку ровно один раз в конце кадра, если были изменения
            if (_isDirty && _isOpen)
            {
                _isDirty = false;
                UpdateUI();
            }
        }
    }
}