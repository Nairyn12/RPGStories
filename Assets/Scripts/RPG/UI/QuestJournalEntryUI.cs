using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.RPG
{
    public class QuestJournalEntryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button button;

        private ActiveQuest _quest;
        private QuestJournalUI _journalUI;

        public void Setup(ActiveQuest quest, QuestJournalUI journalUI)
        {
            _quest = quest;
            _journalUI = journalUI;

            titleText.text = quest.Quest.Title;

            // Подсвечиваем готовые к сдаче квесты желтым
            titleText.color = quest.IsReadyToTurnIn ? Color.yellow : Color.white;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => _journalUI.ShowDetails(_quest));
        }
    }
}