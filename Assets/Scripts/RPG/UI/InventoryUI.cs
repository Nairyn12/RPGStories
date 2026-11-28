using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    public class InventoryUI : MonoBehaviour
    {
        public static InventoryUI Instance { get; private set; }

        [Header("References")]
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private Transform gridParent;
        [SerializeField] private GameObject slotPrefab;

        private List<InventorySlotUI> _spawnedSlots = new();
        public bool IsOpen { get; private set; } = false;

        private PlayerControls _controls;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _controls = new PlayerControls();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.ToggleInventory.performed += OnToggleInventory;
            _controls.Player.Cancel.performed += OnCancelPressed;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.ToggleInventory.performed -= OnToggleInventory;
            _controls.Player.Cancel.performed -= OnCancelPressed;
        }

        private void Start()
        {
            if (inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();
            if (inventory != null) inventory.OnInventoryChanged += MarkDirty;

            if (inventoryPanel != null) inventoryPanel.SetActive(false);
            IsOpen = false;
        }

        private void OnDestroy()
        {
            if (inventory != null) inventory.OnInventoryChanged -= MarkDirty;
        }

        private void OnToggleInventory(InputAction.CallbackContext ctx)
        {
            if (DialogueRunner.IsDialogueActive) return;
            if (LockpickingManager.Instance != null && LockpickingManager.Instance.IsActive) return; // 🛠 НОВАЯ СТРОКА


            // 🛠 ФИКС: Если открыта торговля - закрываем её
            if (TradeUI.Instance != null && TradeUI.Instance.IsOpen)
            {
                TradeUI.Instance.CloseTrade();
                return;
            }

            // Если открыт сундук - закрываем его
            if (ChestUI.Instance != null && ChestUI.Instance.IsOpen)
            {
                ChestUI.Instance.Close();
                return;
            }

            ToggleInventory();
        }

        private void OnCancelPressed(InputAction.CallbackContext ctx)
        {
            if (IsOpen) ToggleInventory();
        }

        public void ToggleInventory()
        {
            if (IsOpen) ForceClose();
            else ForceOpen();
        }

        public void ForceOpen()
        {
            if (IsOpen) return;
            IsOpen = true;
            if (inventoryPanel != null) inventoryPanel.SetActive(true);
            UpdateUI();

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ForceClose()
        {
            if (!IsOpen) return;
            IsOpen = false;

            // 🛠 Прячем тултип при закрытии инвентаря
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();

            // 🛠 Сбрасываем "зависшую" ссылку на наведенный слот
            InventorySlotUI.CurrentlyHoveredSlot = null;

            if (inventoryPanel != null) inventoryPanel.SetActive(false);

            if (ChestUI.Instance == null || !ChestUI.Instance.IsOpen)
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void MarkDirty()
        {
            if (IsOpen) UpdateUI();
        }

        public void UpdateUI()
        {
            if (inventory == null || gridParent == null || slotPrefab == null) return;

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

        // 🛠 НОВЫЙ МЕТОД: Проверка, находится ли курсор над панелью инвентаря
        public bool IsPointerOverInventory(Vector2 screenPos, Camera cam)
        {
            if (inventoryPanel == null || !inventoryPanel.activeSelf) return false;
            RectTransform rect = inventoryPanel.GetComponent<RectTransform>();
            return rect != null && RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, cam);
        }
    }
}