using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.RPG
{
    [Serializable]
    public class ActiveQuest
    {
        public QuestDataSO Quest;
        public List<int> Progress = new List<int>();
        public bool IsReadyToTurnIn = false;

        public bool IsCompleted
        {
            get
            {
                if (Quest == null || Quest.Objectives.Count == 0) return false;
                for (int i = 0; i < Quest.Objectives.Count; i++)
                {
                    if (i >= Progress.Count || Progress[i] < Quest.Objectives[i].RequiredAmount)
                        return false;
                }
                return true;
            }
        }
    }

    public class QuestManager : MonoBehaviour, ISaveable
    {
        public static QuestManager Instance { get; private set; }

        [Header("Active Quests")]
        [SerializeField] private List<ActiveQuest> _activeQuests = new();
        // НОВОЕ: Список сданных квестов, чтобы не брать их повторно
        [SerializeField] private List<QuestDataSO> _completedQuests = new();

        [Header("References")]
        [SerializeField] private PlayerLeveling _playerLeveling;
        [SerializeField] private PlayerInventory _playerInventory;

        public event Action OnQuestsChanged;

        // Публичное свойство для UI (если понадобится)
        public IReadOnlyList<ActiveQuest> ActiveQuests => _activeQuests;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            if (_playerLeveling == null) _playerLeveling = FindFirstObjectByType<PlayerLeveling>();
            if (_playerInventory == null) _playerInventory = FindFirstObjectByType<PlayerInventory>();
        }

        // ==== ВЗЯТИЕ КВЕСТА ====
        public void StartQuest(QuestDataSO quest)
        {
            if (quest == null) return;

            if (_activeQuests.Any(q => q.Quest == quest))
            {
                Debug.LogWarning($"[Quest] Квест '{quest.Title}' уже взят!");
                return;
            }

            var newQuest = new ActiveQuest
            {
                Quest = quest,
                Progress = new List<int>(new int[quest.Objectives.Count]),
                IsReadyToTurnIn = false
            };

            _activeQuests.Add(newQuest);
            Debug.Log($"[Quest] Взят квест: {quest.Title}");
            OnQuestsChanged?.Invoke();
        }

        // ==== ОТСЛЕЖИВАНИЕ УБИЙСТВ ====
        public void ReportKill(string enemyId)
        {
            bool anyQuestChanged = false;

            for (int q = 0; q < _activeQuests.Count; q++)
            {
                var activeQuest = _activeQuests[q];

                // Если квест уже готов к сдаче, не обновляем его прогресс
                if (activeQuest.IsReadyToTurnIn) continue;

                for (int i = 0; i < activeQuest.Quest.Objectives.Count; i++)
                {
                    var obj = activeQuest.Quest.Objectives[i];

                    if (obj.Type == ObjectiveType.KillEnemy && obj.TargetId == enemyId && activeQuest.Progress[i] < obj.RequiredAmount)
                    {
                        activeQuest.Progress[i]++;
                        Debug.Log($"[Quest] {activeQuest.Quest.Title}: {activeQuest.Progress[i]}/{obj.RequiredAmount}");
                        anyQuestChanged = true;
                    }
                }

                // Проверяем, выполнены ли теперь ВСЕ цели
                if (activeQuest.IsCompleted && !activeQuest.IsReadyToTurnIn)
                {
                    activeQuest.IsReadyToTurnIn = true;
                    Debug.Log($"[Quest] Квест '{activeQuest.Quest.Title}' ВЫПОЛНЕН! Вернитесь к NPC для сдачи.");
                }
            }

            if (anyQuestChanged) OnQuestsChanged?.Invoke();
        }

        // Вызывается из PlayerInventory при подборе предмета
        public void ReportCollect(string itemId, int amount)
        {
            bool anyQuestChanged = false;

            for (int q = 0; q < _activeQuests.Count; q++)
            {
                var activeQuest = _activeQuests[q];
                if (activeQuest.IsReadyToTurnIn) continue;

                for (int i = 0; i < activeQuest.Quest.Objectives.Count; i++)
                {
                    var obj = activeQuest.Quest.Objectives[i];

                    if (obj.Type == ObjectiveType.CollectItem && obj.TargetId == itemId)
                    {
                        activeQuest.Progress[i] += amount;
                        // Ограничиваем прогресс, чтобы не показывало "5/3"
                        if (activeQuest.Progress[i] > obj.RequiredAmount)
                            activeQuest.Progress[i] = obj.RequiredAmount;

                        Debug.Log($"[Quest] {activeQuest.Quest.Title}: {activeQuest.Progress[i]}/{obj.RequiredAmount}");
                        anyQuestChanged = true;
                    }
                }

                if (activeQuest.IsCompleted && !activeQuest.IsReadyToTurnIn)
                {
                    activeQuest.IsReadyToTurnIn = true;
                    Debug.Log($"[Quest] Квест '{activeQuest.Quest.Title}' ВЫПОЛНЕН! Вернитесь к NPC.");
                }
            }

            if (anyQuestChanged) OnQuestsChanged?.Invoke();
        }

        // ==== СДАЧА КВЕСТА (Вызывается из диалога) ====
        public void TurnInQuest(QuestDataSO quest)
        {
            if (quest == null) return;

            var activeQuest = _activeQuests.FirstOrDefault(q => q.Quest == quest);

            if (activeQuest == null)
            {
                Debug.LogWarning($"[Quest] Нельзя сдать квест '{quest.Title}', его нет в активных.");
                return;
            }

            if (!activeQuest.IsReadyToTurnIn)
            {
                Debug.LogWarning($"[Quest] Нельзя сдать квест '{quest.Title}', цели еще не выполнены.");
                return;
            }

            // Выдаем награды
            if (quest.RewardXp > 0 && _playerLeveling != null)
            {
                _playerLeveling.AddXp(quest.RewardXp);
                Debug.Log($"[Quest] Получено {quest.RewardXp} XP.");
            }

            if (quest.RewardItem != null && _playerInventory != null)
            {
                _playerInventory.AddItem(quest.RewardItem, quest.RewardItemAmount);
                Debug.Log($"[Quest] Получен предмет: {quest.RewardItem.ItemName} x{quest.RewardItemAmount}.");
            }

            Debug.Log($"[Quest] Квест успешно сдан: {quest.Title}");

            // Удаляем из активных
            _activeQuests.Remove(activeQuest);

            // НОВОЕ: Запоминаем, что квест выполнен
            if (!_completedQuests.Contains(quest))
                _completedQuests.Add(quest);

            OnQuestsChanged?.Invoke();
        }

        // ==== ПРОВЕРКА ДЛЯ ДИАЛОГОВ ====
        public bool IsQuestReadyToTurnIn(QuestDataSO quest)
        {
            var activeQuest = _activeQuests.FirstOrDefault(q => q.Quest == quest);
            return activeQuest != null && activeQuest.IsReadyToTurnIn;
        }

        // Проверяет, взят ли квест (находится ли он в активных)
        public bool IsQuestActive(QuestDataSO quest)
        {
            if (quest == null) return false;
            return _activeQuests.Any(q => q.Quest == quest);
        }

        public bool IsQuestCompleted(QuestDataSO quest)
        {
            return quest != null && _completedQuests.Contains(quest);
        }

        // ==== СОХРАНЕНИЕ (ISaveable) ====
        // (Заглушки, чтобы не было ошибок компиляции. Напишем позже)
        public string SaveKey => "QuestManager";
        public string CaptureState() => "{}";
        public void RestoreState(string json) { }
    }
}