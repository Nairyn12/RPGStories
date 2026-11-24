using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.RPG
{
    public class TradeSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI priceText;

        public PlayerInventory.InventorySlot Slot { get; private set; }
        public bool IsMerchantSlot { get; private set; }
        private TradeUI _tradeUI;

        public void Setup(PlayerInventory.InventorySlot slot, bool isMerchant, int price, TradeUI tradeUI)
        {
            Slot = slot;
            IsMerchantSlot = isMerchant;
            _tradeUI = tradeUI;

            if (Slot == null || Slot.Item == null) return;

            iconImage.gameObject.SetActive(true);
            iconImage.sprite = Slot.Item.Icon;
            iconImage.raycastTarget = false;

            amountText.text = Slot.Amount > 1 ? Slot.Amount.ToString() : "";
            priceText.text = $"{price} g";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Slot == null || Slot.Item == null) return;

            if (IsMerchantSlot)
                _tradeUI.TryBuy(Slot);
            else
                _tradeUI.TrySell(Slot);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Slot != null && Slot.Item != null && TooltipManager.Instance != null)
                TooltipManager.Instance.ShowTooltip(Slot.Item, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
        }
    }
}