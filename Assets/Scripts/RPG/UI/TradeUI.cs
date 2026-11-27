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
        public bool IsOpen { get; private set; } = false;

        [Header("Main Panels")]
        [SerializeField] private GameObject tradePanel;
        [SerializeField] private GameObject characterPanel;
        [SerializeField] private GameObject playerHUD;      // 🛠 НОВОЕ
        [SerializeField] private GameObject hotbarPanel;    // 🛠 НОВОЕ

        [Header("Player UI")]
        [SerializeField] private Transform playerInvGrid;
        [SerializeField] private Transform playerOfferGrid;
        [SerializeField] private Slider playerGoldSlider;
        [SerializeField] private TextMeshProUGUI playerGoldText;

        [Header("Merchant UI")]
        [SerializeField] private Transform merchantInvGrid;
        [SerializeField] private Transform merchantOfferGrid;
        [SerializeField] private Slider merchantGoldSlider;
        [SerializeField] private TextMeshProUGUI merchantGoldText;

        [Header("Deal UI")]
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button balanceButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject tradeSlotPrefab;

        private MerchantInventory _merchant;
        private PlayerInventory _playerInventory;
        private PlayerStats _stats;
        private ReputationManager _rep;

        private List<PlayerInventory.InventorySlot> _playerOffer = new();
        private List<PlayerInventory.InventorySlot> _merchantOffer = new();

        private int _playerGoldAmount;
        private int _merchantGoldAmount;
        private int _playerTotalGold;
        private int _merchantTotalGold;

        private List<GameObject> _spawnedSlots = new();
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

            if (!TradeManager.CanTrade(_rep, _merchant.Faction)) return;

            IsOpen = true;
            tradePanel.SetActive(true);
            if (characterPanel != null) characterPanel.SetActive(false);
            if (playerHUD != null) playerHUD.SetActive(false);       // 🛠 Скрываем HUD
            if (hotbarPanel != null) hotbarPanel.SetActive(false);   // 🛠 Скрываем Хотбар


            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            _playerOffer.Clear();
            _merchantOffer.Clear();
            _playerGoldAmount = 0;
            _merchantGoldAmount = 0;

            SetupSliders();

            balanceButton.onClick.RemoveAllListeners();
            balanceButton.onClick.AddListener(BalanceDeal);

            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(ConfirmTrade);

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseTrade);

            RefreshUI();
        }

        private void SetupSliders()
        {
            _playerTotalGold = _playerInventory.Slots.Where(s => s.Item != null && s.Item.ItemId == "gold").Sum(s => s.Amount);
            _merchantTotalGold = _merchant.Gold;

            playerGoldSlider.minValue = 0;
            playerGoldSlider.maxValue = Mathf.Max(1, _playerTotalGold);
            playerGoldSlider.value = 0;
            playerGoldSlider.onValueChanged.RemoveAllListeners();
            playerGoldSlider.onValueChanged.AddListener((val) => { _playerGoldAmount = (int)val; UpdateDealStatus(); });

            merchantGoldSlider.minValue = 0;
            merchantGoldSlider.maxValue = Mathf.Max(1, _merchantTotalGold);
            merchantGoldSlider.value = 0;
            merchantGoldSlider.onValueChanged.RemoveAllListeners();
            merchantGoldSlider.onValueChanged.AddListener((val) => { _merchantGoldAmount = (int)val; UpdateDealStatus(); });
        }

        public void CloseTrade()
        {
            if (!IsOpen) return;
            IsOpen = false;

            tradePanel.SetActive(false);
            if (characterPanel != null) characterPanel.SetActive(true);
            if (playerHUD != null) playerHUD.SetActive(true);        // 🛠 Возвращаем HUD
            if (hotbarPanel != null) hotbarPanel.SetActive(true);    // 🛠 Возвращаем Хотбар

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        private void RefreshUI()
        {
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();

            foreach (var s in _spawnedSlots) Destroy(s);
            _spawnedSlots.Clear();

            // 1. Инвентарь игрока
            foreach (var slot in _playerInventory.Slots)
            {
                if (slot.EquippedSlot != EquipmentType.None || slot.Item.ItemId == "gold" || _playerOffer.Contains(slot)) continue;

                int price = TradeManager.CalculateSellPrice(slot.Item, _stats, _rep, _merchant.Faction);
                CreateSlot(playerInvGrid, slot, price, () => { _playerOffer.Add(slot); RefreshUI(); });
            }

            // 2. Инвентарь торговца
            foreach (var slot in _merchant.Items)
            {
                if (slot.Amount <= 0) continue; // 🛠 Защита от пустых слотов
                if (slot.Item.IsUnique && _stats.GetStat(StatType.Persuasion) < _merchant.RequiredPersuasionForUnique || _merchantOffer.Contains(slot))
                    continue;

                int price = TradeManager.CalculateBuyPrice(slot.Item, _stats, _rep, _merchant.Faction);
                CreateSlot(merchantInvGrid, slot, price, () => { _merchantOffer.Add(slot); RefreshUI(); });
            }

            // 3. Зона сделки: предметы игрока
            foreach (var slot in _playerOffer)
            {
                int price = TradeManager.CalculateSellPrice(slot.Item, _stats, _rep, _merchant.Faction);
                CreateSlot(playerOfferGrid, slot, price, () => { _playerOffer.Remove(slot); RefreshUI(); }, "X");
            }

            // 4. Зона сделки: предметы торговца
            foreach (var slot in _merchantOffer)
            {
                int price = TradeManager.CalculateBuyPrice(slot.Item, _stats, _rep, _merchant.Faction);
                CreateSlot(merchantOfferGrid, slot, price, () => { _merchantOffer.Remove(slot); RefreshUI(); }, "X");
            }

            UpdateDealStatus();
        }

        private void CreateSlot(Transform parent, PlayerInventory.InventorySlot slot, int price, System.Action onClick, string overrideText = "")
        {
            var obj = Instantiate(tradeSlotPrefab, parent);
            obj.GetComponent<TradeSlotUI>().Setup(slot, price, onClick, overrideText);
            _spawnedSlots.Add(obj);
        }

        private void UpdateDealStatus()
        {
            int playerItemsValue = _playerOffer.Sum(s => TradeManager.CalculateSellPrice(s.Item, _stats, _rep, _merchant.Faction) * s.Amount);
            int merchantItemsValue = _merchantOffer.Sum(s => TradeManager.CalculateBuyPrice(s.Item, _stats, _rep, _merchant.Faction) * s.Amount);

            int difference = merchantItemsValue - playerItemsValue;
            int finalBalance = difference - _playerGoldAmount + _merchantGoldAmount;

            playerGoldText.text = $"Вы даете: {_playerGoldAmount} g";
            merchantGoldText.text = $"Торговец дает: {_merchantGoldAmount} g";

            if (_playerOffer.Count == 0 && _merchantOffer.Count == 0 && _playerGoldAmount == 0 && _merchantGoldAmount == 0)
            {
                statusText.text = "Выберите предметы для сделки.";
                statusText.color = Color.white;
                confirmButton.interactable = false;
                return;
            }

            if (finalBalance > 0)
            {
                statusText.text = $"Вы должны доплатить {finalBalance} g";
                statusText.color = Color.yellow;
                confirmButton.interactable = false;
            }
            else if (finalBalance < 0)
            {
                statusText.text = $"Торговец должен вам {Mathf.Abs(finalBalance)} g";
                statusText.color = Color.yellow;
                confirmButton.interactable = false;
            }
            else
            {
                statusText.text = "Сделка сбалансирована!";
                statusText.color = Color.green;
                confirmButton.interactable = true;
            }
        }

        private void BalanceDeal()
        {
            int playerItemsValue = _playerOffer.Sum(s => TradeManager.CalculateSellPrice(s.Item, _stats, _rep, _merchant.Faction) * s.Amount);
            int merchantItemsValue = _merchantOffer.Sum(s => TradeManager.CalculateBuyPrice(s.Item, _stats, _rep, _merchant.Faction) * s.Amount);

            int difference = merchantItemsValue - playerItemsValue;

            playerGoldSlider.value = 0;
            merchantGoldSlider.value = 0;

            if (difference > 0)
            {
                if (_playerTotalGold >= difference) playerGoldSlider.value = difference;
                else
                {
                    statusText.text = $"Недостаточно золота! Нужно {difference} g, у вас {_playerTotalGold} g.";
                    statusText.color = Color.red;
                }
            }
            else if (difference < 0)
            {
                int amount = Mathf.Abs(difference);
                if (_merchantTotalGold >= amount) merchantGoldSlider.value = amount;
                else
                {
                    statusText.text = $"У торговца недостаточно золота! Нужно {amount} g, у него {_merchantTotalGold} g.";
                    statusText.color = Color.red;
                }
            }
        }

        private void ConfirmTrade()
        {
            if (_playerGoldAmount > _playerTotalGold || _merchantGoldAmount > _merchantTotalGold) return;

            var pOffer = new List<PlayerInventory.InventorySlot>(_playerOffer);
            var mOffer = new List<PlayerInventory.InventorySlot>(_merchantOffer);

            _playerOffer.Clear();
            _merchantOffer.Clear();

            // 🛠 ФИКС: Сохраняем количество в переменную ДО удаления, иначе оно станет 0!
            foreach (var slot in pOffer)
            {
                int amount = slot.Amount;
                _playerInventory.RemoveItem(slot, amount);
                _merchant.AddItem(slot.Item, amount);
            }

            foreach (var slot in mOffer)
            {
                int amount = slot.Amount;
                _merchant.RemoveItem(slot, amount);
                _playerInventory.AddItem(slot.Item, amount);
            }

            // Переводим золото
            if (_playerGoldAmount > 0)
            {
                int amountToRemove = _playerGoldAmount;
                foreach (var goldSlot in _playerInventory.Slots.Where(s => s.Item.ItemId == "gold").ToList())
                {
                    if (amountToRemove <= 0) break;
                    int take = Mathf.Min(goldSlot.Amount, amountToRemove);
                    _playerInventory.RemoveItem(goldSlot, take);
                    amountToRemove -= take;
                }
                _merchant.AddGold(_playerGoldAmount);
            }

            if (_merchantGoldAmount > 0)
            {
                _merchant.RemoveGold(_merchantGoldAmount);
                var goldItem = Resources.LoadAll<ItemDataSO>("Items").FirstOrDefault(i => i.ItemId == "gold");
                if (goldItem != null) _playerInventory.AddItem(goldItem, _merchantGoldAmount);
            }

            _playerGoldAmount = 0;
            _merchantGoldAmount = 0;

            SetupSliders();
            RefreshUI();
            Debug.Log("[Trade] Сделка совершена!");
        }
    }
}