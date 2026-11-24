using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.RPG
{
    public class TradeUI : MonoBehaviour
    {
        public static TradeUI Instance { get; private set; }
        public bool IsOpen { get; private set; } = false; // 🛠 ДОБАВЛЕНО

        [Header("Main Panel")]
        [SerializeField] private GameObject tradePanel;
        [SerializeField] private GameObject characterPanel;

        [Header("Player Side")]
        [SerializeField] private Transform playerGrid;
        [SerializeField] private GameObject tradeSlotPrefab;

        [Header("Merchant Side")]
        [SerializeField] private Transform merchantGrid;
        [SerializeField] private TextMeshProUGUI merchantGoldText;

        [Header("Confirmation Popup")]
        [SerializeField] private GameObject confirmPanel;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;

        private MerchantInventory _merchant;
        private PlayerInventory _playerInventory;
        private PlayerStats _stats;
        private ReputationManager _rep;

        private List<GameObject> _playerSlots = new();
        private List<GameObject> _merchantSlots = new();
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

        private void OnCancelPressed(InputAction.CallbackContext ctx)
        {
            if (IsOpen) CloseTrade();
        }

        public void OpenTrade(MerchantInventory merchant)
        {
            _merchant = merchant;
            _playerInventory = FindFirstObjectByType<PlayerInventory>();
            _stats = FindFirstObjectByType<PlayerStats>();
            _rep = FindFirstObjectByType<ReputationManager>();

            if (!TradeManager.CanTrade(_rep, _merchant.Faction))
            {
                Debug.LogWarning("[Trade] Торговец отказывается с вами торговать (плохая репутация)!");
                return;
            }

            IsOpen = true; // 🛠 УСТАНАВЛИВАЕМ ФЛАГ

            // Закрываем инвентарь через его логику
            if (InventoryUI.Instance != null) InventoryUI.Instance.ForceClose();

            tradePanel.SetActive(true);
            if (characterPanel != null) characterPanel.SetActive(false);
            if (confirmPanel != null) confirmPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            _merchant.OnMerchantChanged += RefreshUI;
            RefreshUI();
        }

        public void CloseTrade()
        {
            if (!IsOpen) return;
            IsOpen = false; // 🛠 СБРАСЫВАЕМ ФЛАГ

            if (_merchant != null) _merchant.OnMerchantChanged -= RefreshUI;

            tradePanel.SetActive(false);
            if (characterPanel != null) characterPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        private void RefreshUI()
        {
            foreach (var s in _playerSlots) Destroy(s);
            _playerSlots.Clear();
            foreach (var s in _merchantSlots) Destroy(s);
            _merchantSlots.Clear();

            if (merchantGoldText != null) merchantGoldText.text = $"Золото: {_merchant.Gold}";

            foreach (var slot in _playerInventory.Slots)
            {
                if (slot.EquippedSlot != EquipmentType.None) continue;
                if (slot.Item.ItemId == "gold") continue;

                int price = TradeManager.CalculateSellPrice(slot.Item, _stats, _rep, _merchant.Faction);
                var obj = Instantiate(tradeSlotPrefab, playerGrid);
                obj.GetComponent<TradeSlotUI>().Setup(slot, false, price, this);
                _playerSlots.Add(obj);
            }

            foreach (var slot in _merchant.Items)
            {
                if (slot.Item.IsUnique && _stats.GetStat(StatType.Persuasion) < _merchant.RequiredPersuasionForUnique)
                    continue;

                int price = TradeManager.CalculateBuyPrice(slot.Item, _stats, _rep, _merchant.Faction);
                var obj = Instantiate(tradeSlotPrefab, merchantGrid);
                obj.GetComponent<TradeSlotUI>().Setup(slot, true, price, this);
                _merchantSlots.Add(obj);
            }
        }

        public void TryBuy(PlayerInventory.InventorySlot slot)
        {
            int price = TradeManager.CalculateBuyPrice(slot.Item, _stats, _rep, _merchant.Faction);

            var goldSlot = _playerInventory.Slots.FirstOrDefault(s => s.Item != null && s.Item.ItemId == "gold");
            int playerGold = goldSlot != null ? goldSlot.Amount : 0;

            if (playerGold < price)
            {
                Debug.LogError("[Trade] Недостаточно золота!");
                return;
            }

            ShowConfirmation($"Купить {slot.Item.ItemName} за {price} золота?", () => {
                _playerInventory.RemoveItem(goldSlot, price);
                _merchant.AddGold(price);
                _merchant.RemoveItem(slot, 1);
                _playerInventory.AddItem(slot.Item, 1);
                RefreshUI();
            });
        }

        public void TrySell(PlayerInventory.InventorySlot slot)
        {
            int price = TradeManager.CalculateSellPrice(slot.Item, _stats, _rep, _merchant.Faction);

            if (_merchant.Gold < price)
            {
                Debug.LogError("[Trade] У торговца недостаточно золота!");
                return;
            }

            ShowConfirmation($"Продать {slot.Item.ItemName} за {price} золота?", () => {
                _merchant.RemoveGold(price);
                var goldItem = Resources.LoadAll<ItemDataSO>("Items").FirstOrDefault(i => i.ItemId == "gold");
                if (goldItem != null) _playerInventory.AddItem(goldItem, price);
                _playerInventory.RemoveItem(slot, 1);
                _merchant.AddItem(slot.Item, 1);
                RefreshUI();
            });
        }

        private System.Action _onConfirmAction;

        private void ShowConfirmation(string text, System.Action onConfirm)
        {
            _onConfirmAction = onConfirm;
            confirmPanel.SetActive(true);
            confirmText.text = text;

            confirmYesButton.onClick.RemoveAllListeners();
            confirmYesButton.onClick.AddListener(() => {
                _onConfirmAction?.Invoke();
                confirmPanel.SetActive(false);
            });

            confirmNoButton.onClick.RemoveAllListeners();
            confirmNoButton.onClick.AddListener(() => {
                confirmPanel.SetActive(false);
            });
        }
    }
}