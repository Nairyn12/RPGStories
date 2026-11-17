using System.Collections.Generic;
using UnityEngine;

namespace Game.RPG
{
    // Типы целей квеста
    public enum ObjectiveType
    {
        KillEnemy,      // Убить врага (например, по EnemyId)
        CollectItem,    // Собрать предмет (по ItemId)
        TalkToNPC,      // Поговорить с NPC
        Custom          // Кастомное событие (для скриптов)
    }

    // Описание одной цели в квесте
    [System.Serializable]
    public class QuestObjective
    {
        public ObjectiveType Type;

        [Tooltip("ID цели: ID врага (например, wolf), ID предмета (heal_potion) или ID NPC")]
        public string TargetId;

        [Tooltip("Сколько раз нужно выполнить (убить 5 волков, собрать 3 травы)")]
        public int RequiredAmount = 1;
    }

    [CreateAssetMenu(fileName = "Quest_", menuName = "RPG/Quest")]
    public class QuestDataSO : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Уникальный ID для сохранений (например, quest_find_healer)")]
        public string QuestId;
        public string Title;
        [TextArea(3, 5)]
        public string Description;

        [Header("Objectives")]
        public List<QuestObjective> Objectives = new();

        [Header("Rewards")]
        public int RewardXp;
        public ItemDataSO RewardItem;
        public int RewardItemAmount = 1;
    }
}