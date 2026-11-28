using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

namespace Game.RPG
{
    public class QuestJournalUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject journalPanel;
        [SerializeField] private Transform questListParent;
        [SerializeField] private GameObject questEntryPrefab;

        [Header("Details")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI objectivesText;

        private PlayerControls _controls;
        private bool _isOpen = false;
        private List<GameObject> _spawnedEntries = new();

        private void Awake() => _controls = new PlayerControls();
        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.ToggleJournal.performed += OnToggleJournal;
            if (QuestManager.Instance != null) QuestManager.Instance.OnQuestsChanged += RefreshList;
        }
        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.ToggleJournal.performed -= OnToggleJournal;
            if (QuestManager.Instance != null) QuestManager.Instance.OnQuestsChanged -= RefreshList;
        }

        private void Start()
        {
            journalPanel.SetActive(false);
            ClearDetails();
        }

        private void OnToggleJournal(InputAction.CallbackContext ctx)
        {
            if (DialogueRunner.IsDialogueActive) return; // Не открываем во время диалога
            if (LockpickingManager.Instance != null && LockpickingManager.Instance.IsActive) return; // 🛠 НОВАЯ СТРОКА

            _isOpen = !_isOpen;
            journalPanel.SetActive(_isOpen);

            if (_isOpen)
            {
                RefreshList();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
        }

        public void RefreshList()
        {
            foreach (var entry in _spawnedEntries) Destroy(entry);
            _spawnedEntries.Clear();
            ClearDetails();

            if (QuestManager.Instance == null) return;

            foreach (var activeQuest in QuestManager.Instance.ActiveQuests)
            {
                GameObject entryObj = Instantiate(questEntryPrefab, questListParent);
                var entryUI = entryObj.GetComponent<QuestJournalEntryUI>();
                if (entryUI != null)
                {
                    entryUI.Setup(activeQuest, this);
                    _spawnedEntries.Add(entryObj);
                }
            }
        }

        public void ShowDetails(ActiveQuest activeQuest)
        {
            if (activeQuest == null) return;

            titleText.text = activeQuest.Quest.Title;
            descText.text = activeQuest.Quest.Description;
            objectivesText.text = "";

            for (int i = 0; i < activeQuest.Quest.Objectives.Count; i++)
            {
                var obj = activeQuest.Quest.Objectives[i];
                int current = activeQuest.Progress[i];
                string objName = obj.TargetId; // В идеале тут должно быть читаемое имя

                string color = (current >= obj.RequiredAmount) ? "green" : "white";
                objectivesText.text += $"- <color={color}>{objName}: {current}/{obj.RequiredAmount}</color>\n";
            }
        }

        private void ClearDetails()
        {
            titleText.text = "Выберите квест";
            descText.text = "";
            objectivesText.text = "";
        }
    }
}