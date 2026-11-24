using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    public class ChestUI : MonoBehaviour
    {
        public static ChestUI Instance { get; private set; }

        [Header("References")]
        [SerializeField] private GameObject chestPanel;
        [SerializeField] private Transform chestGridParent;
        [SerializeField] private GameObject chestSlotPrefab;
        [SerializeField] private GameObject characterPanel;

        public bool IsOpen { get; private set; } = false;
        public Chest CurrentChest => _currentChest; // 🛠 Доступ для Drag & Drop

        private Chest _currentChest;
        private List<ChestSlotUI> _spawnedSlots = new();
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
            _controls.Player.Cancel.performed += OnCancelPressed;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.Cancel.performed -= OnCancelPressed;
        }

        private void Start()
        {
            if (chestPanel != null) chestPanel.SetActive(false);
        }

        private void OnCancelPressed(InputAction.CallbackContext ctx)
        {
            if (IsOpen) Close();
        }

        public void Open(Chest chest)
        {
            if (_currentChest != null) _currentChest.OnChestChanged -= RefreshUI;

            _currentChest = chest;
            _currentChest.OnChestChanged += RefreshUI;
            IsOpen = true;

            RefreshUI();

            if (chestPanel != null) chestPanel.SetActive(true);
            if (InventoryUI.Instance != null) InventoryUI.Instance.ForceOpen();
            if (characterPanel != null) characterPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        public void Close()
        {
            if (!IsOpen) return;

            if (_currentChest != null) _currentChest.OnChestChanged -= RefreshUI;
            _currentChest = null;
            IsOpen = false;

            // 🛠 Прячем тултип
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
            InventorySlotUI.CurrentlyHoveredSlot = null;

            if (chestPanel != null) chestPanel.SetActive(false);

            if (InventoryUI.Instance != null) InventoryUI.Instance.ForceClose();
            if (characterPanel != null) characterPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        private void RefreshUI()
        {
            foreach (var slot in _spawnedSlots) Destroy(slot.gameObject);
            _spawnedSlots.Clear();

            if (_currentChest == null) return;

            foreach (var slotData in _currentChest.Slots)
            {
                GameObject slotObj = Instantiate(chestSlotPrefab, chestGridParent);
                if (slotObj.TryGetComponent(out ChestSlotUI slotUI))
                {
                    slotUI.Setup(slotData, _currentChest);
                    _spawnedSlots.Add(slotUI);
                }
            }

            if (InventoryUI.Instance != null) InventoryUI.Instance.UpdateUI();
        }

        // 🛠 НОВЫЙ МЕТОД: Проверка, находится ли курсор над панелью сундука
        public bool IsPointerOverChest(Vector2 screenPos, Camera cam)
        {
            if (chestPanel == null || !chestPanel.activeSelf) return false;
            RectTransform rect = chestPanel.GetComponent<RectTransform>();
            return rect != null && RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, cam);
        }
    }
}