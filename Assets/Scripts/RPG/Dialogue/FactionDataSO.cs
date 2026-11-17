using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Faction_", menuName = "RPG/Faction")]
    public class FactionDataSO : ScriptableObject
    {
        public string FactionId; // <-- НОВОЕ: Например, "village"
        public string FactionName;
        public Sprite Icon; // Пригодится для UI
    }
}