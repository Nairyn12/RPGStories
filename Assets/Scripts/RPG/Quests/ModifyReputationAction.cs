using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Action_ModifyRep_", menuName = "RPG/Dialogue/Action - Modify Reputation")]
    public class ModifyReputationAction : DialogueAction
    {
        public FactionDataSO Faction;
        public int Amount;

        public override void Execute(GameObject player)
        {
            var repManager = FindFirstObjectByType<ReputationManager>();
            if (repManager != null && Faction != null)
            {
                repManager.ModifyReputation(Faction, Amount);
            }
        }
    }
}