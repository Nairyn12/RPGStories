using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Game.RPG
{
    public class TradeSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI priceText;

        private PlayerInventory.InventorySlot _slot;
        private Action _onClick;
        private string _priceOverride;

        public void Setup(PlayerInventory.InventorySlot slot, int price, Action onClick, string priceOverride = "")
        {
            _slot = slot;
            _onClick = onClick;
            _priceOverride = priceOverride;

            if (_slot == null || _slot.Item == null)
            {
                if (iconImage != null) iconImage.gameObject.SetActive(false);
                if (amountText != null) amountText.text = "";
                if (priceText != null) priceText.text = "";
                return;
            }

            if (iconImage != null)
            {
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = _slot.Item.Icon;
                iconImage.raycastTarget = false;
                iconImage.preserveAspect = true; // 🛠 Чтобы иконка не растягивалась
            }

            if (amountText != null)
                amountText.text = _slot.Amount > 1 ? _slot.Amount.ToString() : "";

            if (priceText != null)
            {
                if (string.IsNullOrEmpty(_priceOverride))
                    priceText.text = $"{price} g";
                else
                    priceText.text = _priceOverride;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 🛠 ФИКС: Скрываем тултип при клике, чтобы он не залипал
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
            _onClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_slot != null && _slot.Item != null && TooltipManager.Instance != null)
                TooltipManager.Instance.ShowTooltip(_slot.Item, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
        }
    }
}