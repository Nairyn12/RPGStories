using UnityEngine;

namespace Game.RPG
{
    public abstract class DialogueCondition : ScriptableObject
    {
        public abstract bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction);
    }
}