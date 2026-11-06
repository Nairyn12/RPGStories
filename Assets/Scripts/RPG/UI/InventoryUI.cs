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
            if (inventory != null)
                inventory.OnInventoryChanged += UpdateUI;

            inventoryPanel.SetActive(false);
            _isOpen = false;
        }

        private void OnDestroy()
        {
            if (inventory != null)
                inventory.OnInventoryChanged -= UpdateUI;
        }

        // Обработчик нажатия кнопки
        private void OnToggleInventory(InputAction.CallbackContext ctx)
        {
            ToggleInventory();
        }

        public void ToggleInventory()
        {
            if (inventoryPanel == null)
            {
                Debug.LogError("[InventoryUI] Поле Inventory Panel не назначено в инспекторе!");
                return;
            }

            _isOpen = !_isOpen;
            inventoryPanel.SetActive(_isOpen);

            if (_isOpen)
            {
                UpdateUI();
                Time.timeScale = 0f; // Останавливаем время
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("[InventoryUI] Инвентарь открыт.");
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log("[InventoryUI] Инвентарь закрыт.");
            }
        }

        private void UpdateUI()
        {
            // 1. Очищаем старые слоты (открепляем их, чтобы сетка не видела их)
            foreach (Transform child in gridParent)
            {
                child.SetParent(null); // Мгновенно убираем из сетки
                Destroy(child.gameObject); // Удаляем в конце кадра
            }
            _spawnedSlots.Clear();

            // 2. Создаем новые слоты на основе данных инвентаря
            foreach (var slotData in inventory.Slots)
            {
                GameObject slotObj = Instantiate(slotPrefab, gridParent);
                if (slotObj.TryGetComponent(out InventorySlotUI slotUI))
                {
                    slotUI.Setup(slotData);
                    slotUI.OnSlotClicked += HandleSlotClicked;
                    _spawnedSlots.Add(slotUI);
                }
            }
        }

        private void HandleSlotClicked(InventorySlotUI slot)
        {
            Debug.Log($"[UI] Клик по предмету: {slot.Item.ItemName}");
        }
    }
}