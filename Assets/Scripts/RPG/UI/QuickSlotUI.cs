using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.RPG
{
    public class QuickSlotUI : MonoBehaviour, IPointerClickHandler, IDropHandler
    {
        [SerializeField] private int slotIndex;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Outline borderImage;

        private PlayerInventory _inventory;

        private void Start()
        {
            if (amountText != null) amountText.text = (slotIndex + 1).ToString();
            UpdateUI();
        }

        private void OnEnable()
        {
            _inventory = FindFirstObjectByType<PlayerInventory>();
            if (_inventory != null) _inventory.OnInventoryChanged += UpdateUI;
        }

        private void OnDisable()
        {
            if (_inventory != null) _inventory.OnInventoryChanged -= UpdateUI;
        }

        public void UpdateUI()
        {
            if (_inventory == null) return;

            var slot = _inventory.GetQuickSlot(slotIndex);

            if (slot != null && slot.Item != null)
            {
                if (iconImage != null) // Защита
                {
                    iconImage.gameObject.SetActive(true);
                    iconImage.sprite = slot.Item.Icon;
                    iconImage.raycastTarget = false;
                }

                if (borderImage != null) borderImage.enabled = slot.EquippedSlot != EquipmentType.None;
            }
            else
            {
                if (iconImage != null) iconImage.gameObject.SetActive(false);
                if (borderImage != null) borderImage.enabled = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_inventory == null) return;

            if (eventData.button == PointerEventData.InputButton.Left)
                _inventory.UseQuickSlot(slotIndex);
            else if (eventData.button == PointerEventData.InputButton.Right)
                _inventory.ClearQuickSlot(slotIndex);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_inventory == null) return;
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out InventorySlotUI invSlotUI))
            {
                if (invSlotUI.Slot != null && invSlotUI.Slot.Item != null)
                {
                    // 🛠 НОВАЯ ПРОВЕРКА: Если предмет нельзя класть в хотбар - выходим
                    if (!invSlotUI.Slot.Item.CanBeInHotbar)
                    {
                        Debug.LogWarning($"[Hotbar] Предмет {invSlotUI.Slot.Item.ItemName} нельзя положить в хотбар!");
                        return;
                    }

                    _inventory.SetQuickSlot(slotIndex, invSlotUI.Slot);
                }
            }
        }
    }
}