using System.Collections.Generic;
using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Armor_", menuName = "RPG/Items/Armor")]
    public class ArmorItemSO : ItemDataSO
    {
        [Header("Stat Modifiers")]
        [Tooltip(" акие статы мен€ет эта брон€ (например, +2 к скрытности)")]
        public List<StatModifier> Modifiers = new();
    }
}
