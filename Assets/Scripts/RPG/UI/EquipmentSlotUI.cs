using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.RPG
{
    public class EquipmentSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private EquipmentType slotType;
        [SerializeField] private Image iconImage;
        [SerializeField] private Outline borderImage;

        private PlayerInventory _inventory;

        private void OnEnable()
        {
            _inventory = FindFirstObjectByType<PlayerInventory>();
            if (_inventory != null) _inventory.OnEquipmentChanged += UpdateUI;

            // 🛠 ФИКС: Принудительно обновляем иконку, когда панель включается
            UpdateUI();
        }

        private void OnDisable()
        {
            if (_inventory != null) _inventory.OnEquipmentChanged -= UpdateUI;
        }

        public void UpdateUI()
        {
            if (_inventory == null || iconImage == null) return;

            var equippedSlot = _inventory.GetEquippedSlot(slotType);

            if (equippedSlot != null && equippedSlot.Item != null)
            {
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = equippedSlot.Item.Icon;
                iconImage.raycastTarget = false;

                RectTransform rt = iconImage.GetComponent<RectTransform>();
                rt.anchoredPosition = Vector2.zero;

                if (borderImage != null) borderImage.enabled = true;
            }
            else
            {
                iconImage.gameObject.SetActive(false);
                if (borderImage != null) borderImage.enabled = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_inventory != null && _inventory.GetEquippedSlot(slotType) != null)
            {
                _inventory.UnequipItem(slotType);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_inventory == null) return;
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out InventorySlotUI invSlotUI))
            {
                if (invSlotUI.Slot != null && invSlotUI.Slot.Item != null && invSlotUI.Slot.Item.EquipSlot == slotType)
                {
                    _inventory.EquipItem(invSlotUI.Slot);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var slot = _inventory?.GetEquippedSlot(slotType);
            if (slot != null && slot.Item != null && TooltipManager.Instance != null)
                TooltipManager.Instance.ShowTooltip(slot.Item, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
        }
    }
}