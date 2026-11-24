using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Action_StartTrade_", menuName = "RPG/Dialogue/Action - Start Trade")]
    public class StartTradeAction : DialogueAction
    {
        public override void Execute(GameObject player)
        {
            // В диалоге нам нужно найти NPC, с которым мы говорим. 
            // Поскольку DialogueRunner не передает NPC, найдем ближайший MerchantInventory
            var merchant = FindFirstObjectByType<MerchantInventory>();
            if (merchant != null && TradeUI.Instance != null)
            {
                // Закрываем диалог перед открытием торговли
                if (DialogueRunner.Instance != null) DialogueRunner.Instance.EndDialogue();

                TradeUI.Instance.OpenTrade(merchant);
            }
        }
    }
}