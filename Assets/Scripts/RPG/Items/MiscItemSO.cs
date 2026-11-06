using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Misc_", menuName = "RPG/Items/Misc or Quest")]
    public class MiscItemSO : ItemDataSO
    {
        // Здесь можно добавить специфичные поля для квестов, 
        // например bool IsQuestItem = true;
        // Пока оставим пустым, базы достаточно.
    }
}