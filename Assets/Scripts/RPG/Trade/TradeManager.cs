using UnityEngine;

namespace Game.RPG
{
    public static class TradeManager
    {
        // Вычисляем цену покупки
        public static int CalculateBuyPrice(ItemDataSO item, PlayerStats stats, ReputationManager rep, FactionDataSO faction)
        {
            float price = item.BasePrice;

            // 1. Навык Торговли (снижает цену на 1% за уровень)
            float tradeSkill = stats.GetStat(StatType.Trade);
            price *= (1f - (tradeSkill * 0.01f));

            // 2. Репутация
            int repValue = rep.GetReputation(faction);
            if (repValue > 0)
                price *= (1f - (repValue * 0.002f)); // Друг: скидка
            else if (repValue < 0)
                price *= (1f + (Mathf.Abs(repValue) * 0.005f)); // Враг: наценка

            return Mathf.Max(1, Mathf.RoundToInt(price));
        }

        // Вычисляем цену продажи
        public static int CalculateSellPrice(ItemDataSO item, PlayerStats stats, ReputationManager rep, FactionDataSO faction)
        {
            float price = item.BasePrice * 0.4f; // Базовая цена продажи - 40% от стоимости

            // 1. Навык Торговли (повышает цену продажи на 1% за уровень)
            float tradeSkill = stats.GetStat(StatType.Trade);
            price *= (1f + (tradeSkill * 0.01f));

            // 2. Репутация
            int repValue = rep.GetReputation(faction);
            if (repValue > 0)
                price *= (1f + (repValue * 0.002f));
            else if (repValue < 0)
                price *= (1f - (Mathf.Abs(repValue) * 0.005f));

            return Mathf.Max(1, Mathf.RoundToInt(price));
        }

        // Проверка, может ли игрок торговать (низкая репутация блокирует)
        public static bool CanTrade(ReputationManager rep, FactionDataSO faction)
        {
            if (faction == null) return true;
            return rep.GetReputation(faction) > -50; // Если репутация ниже -50, торговец откажет
        }
    }
}