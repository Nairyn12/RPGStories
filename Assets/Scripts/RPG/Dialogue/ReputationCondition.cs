using UnityEngine;

namespace Game.RPG
{
    public enum ComparisonType { GreaterOrEqual, LessOrEqual }

    [CreateAssetMenu(fileName = "Cond_Reputation_", menuName = "RPG/Dialogue/Condition - Reputation")]
    public class ReputationCondition : DialogueCondition
    {
        public int RequiredValue;
        public ComparisonType Comparison = ComparisonType.GreaterOrEqual;

        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
        {
            if (npcFaction == null) return false;
            var repManager = FindFirstObjectByType<ReputationManager>();
            if (repManager == null) return false;

            int currentRep = repManager.GetReputation(npcFaction);

            if (Comparison == ComparisonType.GreaterOrEqual)
                return currentRep >= RequiredValue;
            else
                return currentRep <= RequiredValue;
        }
    }
}