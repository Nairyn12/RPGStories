using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.RPG
{
    public class ChestSlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Outline borderImage;

        public PlayerInventory.InventorySlot Slot { get; private set; }
        private Chest _chest;
        private GameObject _dragIcon;

        public void Setup(PlayerInventory.InventorySlot slot, Chest chest)
        {
            Slot = slot;
            _chest = chest;

            if (Slot == null || Slot.Item == null) return;

            iconImage.gameObject.SetActive(true);
            iconImage.sprite = Slot.Item.Icon;
            iconImage.raycastTarget = false;
            amountText.text = Slot.Amount > 1 ? Slot.Amount.ToString() : "";
            amountText.raycastTarget = false;
        }

        // Быстрое перемещение по клику (в инвентарь)
        public void OnPointerClick(PointerEventData eventData)
        {
            if (Slot == null || Slot.Item == null) return;

            var inventory = FindFirstObjectByType<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(Slot.Item, Slot.Amount);
                _chest.RemoveItem(Slot, Slot.Amount);
            }
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

        // ==== DRAG & DROP ====
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Slot == null || Slot.Item == null) return;
            TooltipManager.Instance.HideTooltip();
            iconImage.color = new Color(1, 1, 1, 0.5f);

            _dragIcon = new GameObject("DragIcon_Chest");
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
            iconImage.color = Color.white;

            if (Slot == null || Slot.Item == null) return;

            // Если мы бросили предмет НЕ на этот слот и НЕ на инвентарь — ничего не делаем (возврат)
            if (eventData.pointerEnter == null) return;

            // Если бросили на инвентарь — перемещаем туда
            if (eventData.pointerEnter.TryGetComponent(out InventorySlotUI invSlotUI))
            {
                var inventory = FindFirstObjectByType<PlayerInventory>();
                if (inventory != null)
                {
                    inventory.AddItem(Slot.Item, Slot.Amount);
                    _chest.RemoveItem(Slot, Slot.Amount);
                }
            }
        }
    }
}