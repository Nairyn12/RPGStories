using TMPro;
using UnityEngine;

namespace Game.RPG
{
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager Instance { get; private set; }

        [SerializeField] private GameObject tooltipPanel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI statsText;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            HideTooltip();
        }

        public void ShowTooltip(ItemDataSO item, Vector2 position)
        {
            if (item == null || tooltipPanel == null) return;

            tooltipPanel.SetActive(true);
            titleText.text = item.ItemName;
            descText.text = item.Description;

            statsText.text = "";
            if (item is ArmorItemSO armor)
            {
                foreach (var mod in armor.Modifiers)
                    statsText.text += $"{mod.Stat}: +{mod.Value}\n";
            }
            else if (item is WeaponItemSO weapon)
            {
                statsText.text += $"Урон: {weapon.BaseDamage}\n";
            }

            // Двигаем панель (делаем небольшой отступ от курсора)
            tooltipPanel.transform.position = position + new Vector2(20, -20);
        }

        public void HideTooltip()
        {
            if (tooltipPanel != null) tooltipPanel.SetActive(false);
        }
    }
}
