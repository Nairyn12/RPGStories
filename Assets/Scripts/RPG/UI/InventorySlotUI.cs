using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.RPG
{
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI Elements")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;

        private ItemDataSO _item;
        private int _amount;

        // Событие при клике на слот (передается слот и сама UI-кнопка)
        public System.Action<InventorySlotUI> OnSlotClicked;

        public ItemDataSO Item => _item;

        public void Setup(PlayerInventory.InventorySlot slot)
        {
            if (slot == null || slot.Item == null)
            {
                _item = null;
                _amount = 0;
                iconImage.gameObject.SetActive(false);
                amountText.text = "";
                return;
            }

            _item = slot.Item;
            _amount = slot.Amount;

            iconImage.gameObject.SetActive(true);
            iconImage.sprite = _item.Icon;

            if (_amount > 1)
                amountText.text = _amount.ToString();
            else
                amountText.text = "";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_item != null)
            {
                OnSlotClicked?.Invoke(this);
            }
        }
    }
}