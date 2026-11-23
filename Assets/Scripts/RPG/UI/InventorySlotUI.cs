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
        [SerializeField] private Outline borderImage;        // <-- Синяя рамка
        [SerializeField] private TextMeshProUGUI hotbarText; // <-- Циферка хотбара

        public static InventorySlotUI CurrentlyHoveredSlot { get; private set; }

        public PlayerInventory.InventorySlot Slot { get; private set; }

        private PlayerInventory _inventory;
        private GameObject _dragIcon;

        public void Setup(PlayerInventory.InventorySlot slot, PlayerInventory inventory)
        {
            _inventory = inventory;
            Slot = slot;

            if (Slot == null || Slot.Item == null) return;

            iconImage.gameObject.SetActive(true);
            iconImage.sprite = Slot.Item.Icon;
            iconImage.raycastTarget = false;

            amountText.text = Slot.Amount > 1 ? Slot.Amount.ToString() : "";
            amountText.raycastTarget = false;

            // Визуал: Рамка экипировки и цифра хотбара
            if (borderImage != null) borderImage.enabled = Slot.EquippedSlot != EquipmentType.None;
            if (hotbarText != null)
            {
                hotbarText.gameObject.SetActive(Slot.HotbarIndex != -1);
                if (Slot.HotbarIndex != -1) hotbarText.text = (Slot.HotbarIndex + 1).ToString(); // +1 чтобы было 1-8, а не 0-7
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Slot == null || Slot.Item == null) return;
            if (eventData.clickCount == 2) _inventory.UseItem(Slot);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CurrentlyHoveredSlot = this; // <-- Запоминаем слот

            if (Slot != null && Slot.Item != null && TooltipManager.Instance != null)
                TooltipManager.Instance.ShowTooltip(Slot.Item, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Если мышка ушла с этого слота - очищаем
            if (CurrentlyHoveredSlot == this) CurrentlyHoveredSlot = null;

            if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip();
        }

        // ==== DRAG & DROP ====
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Slot == null || Slot.Item == null) return;
            TooltipManager.Instance.HideTooltip();
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
            if (Slot == null || Slot.Item == null) return;

            // Если мы попали в слот сундука — перемещаем туда
            if (eventData.pointerEnter != null && eventData.pointerEnter.TryGetComponent(out ChestSlotUI chestSlot))
            {
                var chest = FindFirstObjectByType<ChestUI>(); // Или передать через ссылку
                if (chest != null && chestSlot.Slot != null)
                {
                    // Находим сам сундук через UI
                    var currentChest = chestSlot.GetComponentInParent<ChestUI>();
                    // Упрощенно: просто добавим в сундук через InventorySlotUI
                    var chestComponent = chestSlot.Slot.Item != null ? chestSlot.GetComponentInParent<Chest>() : null;
                    if (chestComponent != null)
                    {
                        chestComponent.AddItem(Slot.Item, Slot.Amount);
                        _inventory.RemoveItem(Slot, Slot.Amount);
                    }
                }
            }

            // Если мышь не над UI (над панелями) - выбрасываем
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _inventory.DropItem(Slot, 1);
            }
        }
    }
}