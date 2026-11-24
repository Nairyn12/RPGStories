using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.RPG
{
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI Elements")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Outline borderImage;
        [SerializeField] private TextMeshProUGUI hotbarText;

        public PlayerInventory.InventorySlot Slot { get; private set; }
        public static InventorySlotUI CurrentlyHoveredSlot { get; set; }

        private PlayerInventory _inventory;
        private GameObject _dragIcon;

        public void Setup(PlayerInventory.InventorySlot slot, PlayerInventory inventory)
        {
            _inventory = inventory;
            Slot = slot;

            if (Slot == null || Slot.Item == null)
            {
                if (iconImage != null) iconImage.gameObject.SetActive(false);
                if (amountText != null) amountText.text = "";
                if (borderImage != null) borderImage.enabled = false;
                if (hotbarText != null) hotbarText.gameObject.SetActive(false);
                return;
            }

            iconImage.gameObject.SetActive(true);
            iconImage.sprite = Slot.Item.Icon;
            iconImage.raycastTarget = false;
            amountText.text = Slot.Amount > 1 ? Slot.Amount.ToString() : "";
            amountText.raycastTarget = false;

            if (borderImage != null) borderImage.enabled = Slot.EquippedSlot != EquipmentType.None;

            if (hotbarText != null)
            {
                hotbarText.gameObject.SetActive(Slot.HotbarIndex != -1);
                if (Slot.HotbarIndex != -1) hotbarText.text = (Slot.HotbarIndex + 1).ToString();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
            if (Slot == null || Slot.Item == null) return;

            if (eventData.clickCount == 2) _inventory.UseItem(Slot);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CurrentlyHoveredSlot = this;
            if (Slot != null && Slot.Item != null && TooltipManager.Instance != null)
                TooltipManager.Instance.ShowTooltip(Slot.Item, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (CurrentlyHoveredSlot == this) CurrentlyHoveredSlot = null;
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
            if (Slot == null || Slot.Item == null) return;

            iconImage.color = new Color(1, 1, 1, 0.5f);

            _dragIcon = new GameObject("DragIcon");
            _dragIcon.transform.SetParent(GetComponentInParent<Canvas>().transform, false);
            Image img = _dragIcon.AddComponent<Image>();
            img.sprite = Slot.Item.Icon;
            img.raycastTarget = false;

            RectTransform rect = _dragIcon.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(80, 80);
            _dragIcon.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragIcon != null) _dragIcon.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragIcon != null) Destroy(_dragIcon);
            if (iconImage != null) iconImage.color = Color.white;
            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();

            if (Slot == null || Slot.Item == null) return;

            // 1. Бросили на слот экипировки
            if (eventData.pointerEnter != null && eventData.pointerEnter.TryGetComponent(out EquipmentSlotUI eqSlot)) return;

            // 2. Бросили в хотбар
            if (eventData.pointerEnter != null && eventData.pointerEnter.TryGetComponent(out QuickSlotUI quickSlot)) return;

            // 🛠 3. Бросили в сундук (проверяем по координатам, работает даже на пустую панель)
            if (ChestUI.Instance != null && ChestUI.Instance.IsOpen && ChestUI.Instance.IsPointerOverChest(eventData.position, eventData.pressEventCamera))
            {
                var chest = ChestUI.Instance.CurrentChest;
                if (chest != null)
                {
                    chest.AddItem(Slot.Item, Slot.Amount);
                    _inventory.RemoveItem(Slot, Slot.Amount);
                    return;
                }
            }

            // 4. Бросили на другой слот инвентаря (пока игнорируем)
            if (eventData.pointerEnter != null && eventData.pointerEnter.TryGetComponent(out InventorySlotUI invSlot)) return;

            // 5. Бросили в пустоту вне UI (выброс на землю)
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _inventory.DropItem(Slot, 1);
            }
        }
    }
}