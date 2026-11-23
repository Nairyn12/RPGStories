using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    public class ChestUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject chestPanel;
        [SerializeField] private Transform chestGridParent;
        [SerializeField] private GameObject chestSlotPrefab;
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private GameObject inventoryPanel;

        private Chest _currentChest;
        private List<ChestSlotUI> _spawnedSlots = new();
        private PlayerControls _controls;

        private void Awake()
        {
            _controls = new PlayerControls();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
            // 🛠 ИСПРАВЛЕНО: Подписываемся на отдельный метод
            _controls.Player.Cancel.performed += OnCancelPressed;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            // 🛠 ИСПРАВЛЕНО: Отписываемся от метода
            _controls.Player.Cancel.performed -= OnCancelPressed;
        }

        // 🛠 ИСПРАВЛЕНО: Метод-обработчик для Esc
        private void OnCancelPressed(InputAction.CallbackContext ctx)
        {
            Close();
        }

        private void Start()
        {
            if (chestPanel != null) chestPanel.SetActive(false);
        }

        public void Open(Chest chest)
        {
            if (_currentChest != null) _currentChest.OnChestChanged -= RefreshUI;

            _currentChest = chest;
            _currentChest.OnChestChanged += RefreshUI;

            RefreshUI();

            if (chestPanel != null) chestPanel.SetActive(true);
            if (inventoryPanel != null) inventoryPanel.SetActive(true); // Показываем инвентарь рядом

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        public void Close()
        {
            if (_currentChest != null) _currentChest.OnChestChanged -= RefreshUI;
            _currentChest = null;

            if (chestPanel != null) chestPanel.SetActive(false);
            if (inventoryPanel != null) inventoryPanel.SetActive(false); // Закрываем инвентарь

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
        }
    }
}