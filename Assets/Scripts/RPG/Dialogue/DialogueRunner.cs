using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    public class DialogueRunner : MonoBehaviour
    {
        public static DialogueRunner Instance { get; private set; }
        public static bool IsDialogueActive { get; private set; } = false;

        [SerializeField] private DialogueUI ui;
        [SerializeField] private PlayerController playerController;

        private DialogueSO _currentDialogue;
        private int _currentNodeIndex;
        private GameObject _playerObj;
        private FactionDataSO _currentNpcFaction;

        private PlayerControls _controls;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _controls = new PlayerControls();
        }

        private void OnEnable()
        {
            if (_controls != null) _controls.Player.Enable();
        }
        private void OnDisable()
        {
            if (_controls != null) _controls.Player.Disable();
        }

        private void Update()
        {
            if (IsDialogueActive && _controls.Player.Cancel.triggered)
            {
                EndDialogue();
            }
        }

        public void StartDialogue(DialogueSO dialogue, GameObject player, FactionDataSO npcFaction)
        {
            if (dialogue == null || dialogue.Nodes.Count == 0) return;

            // АВТОПОИСК (так как Manager переживает загрузку сцены)
            if (ui == null) ui = FindFirstObjectByType<DialogueUI>();
            if (playerController == null) playerController = FindFirstObjectByType<PlayerController>();

            IsDialogueActive = true;
            _playerObj = player;
            _currentDialogue = dialogue;
            _currentNpcFaction = npcFaction;

            if (playerController != null) playerController.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ui.Show(dialogue);

            // 🛠 Ищем правильный стартовый узел
            _currentNodeIndex = FindStartNodeIndex();

            if (_currentNodeIndex == -1)
            {
                // Если ни один узел не подошел (нет дефолтного)
                EndDialogue();
                return;
            }

            DisplayNode(_currentNodeIndex);
        }

        // Логика поиска первого подходящего узла
        private int FindStartNodeIndex()
        {
            if (_currentDialogue == null || _currentDialogue.Nodes == null || _currentDialogue.Nodes.Count == 0)
                return -1;

            var stats = _playerObj != null ? _playerObj.GetComponent<PlayerStats>() : null;
            if (stats == null) stats = FindFirstObjectByType<PlayerStats>();

            var inv = _playerObj != null ? _playerObj.GetComponent<PlayerInventory>() : null;
            if (inv == null) inv = FindFirstObjectByType<PlayerInventory>();

            for (int i = 0; i < _currentDialogue.Nodes.Count; i++)
            {
                var node = _currentDialogue.Nodes[i];
                if (node == null) continue;

                bool isAvailable = true;

                if (node.Conditions != null)
                {
                    foreach (var condition in node.Conditions)
                    {
                        if (condition == null) continue; // Игнорируем пустые условия в инспекторе

                        if (!condition.IsMet(stats, inv, _currentNpcFaction))
                        {
                            isAvailable = false;
                            break;
                        }
                    }
                }

                if (isAvailable) return i;
            }

            return -1;
        }

        private void DisplayNode(int index)
        {
            if (index < 0 || index >= _currentDialogue.Nodes.Count)
            {
                EndDialogue();
                return;
            }

            _currentNodeIndex = index;
            var node = _currentDialogue.Nodes[index];

            ExecuteActions(node.OnEnterActions);
            ui.ClearChoices();
            ui.SetText(node.NpcText);

            var stats = _playerObj.GetComponent<PlayerStats>() ?? FindFirstObjectByType<PlayerStats>();
            var inv = _playerObj.GetComponent<PlayerInventory>() ?? FindFirstObjectByType<PlayerInventory>();

            if (node.Choices.Count > 0)
            {
                foreach (var choice in node.Choices)
                {
                    if (choice == null) continue; // Игнорируем пустые варианты ответа

                    bool isAvailable = true;
                    if (choice.Conditions != null)
                    {
                        foreach (var condition in choice.Conditions)
                        {
                            if (condition == null) continue; // Игнорируем пустые условия

                            if (!condition.IsMet(stats, inv, _currentNpcFaction)) { isAvailable = false; break; }
                        }
                    }

                    ui.SpawnChoiceButton(choice.ChoiceText, choice.NextNodeIndex, isAvailable, (targetIdx) => {
                        ExecuteActions(choice.Actions);
                        DisplayNode(targetIdx);
                    });
                }

                ui.SpawnChoiceButton("[Уйти]", -1, true, (targetIdx) => EndDialogue());
            }
            else
            {
                // Если у узла нет выборов, проверяем, не является ли он конечным
                if (node.EndDialogueHere)
                {
                    ui.SpawnChoiceButton("[Закончить разговор]", -1, true, (targetIdx) => EndDialogue());
                    return; // Выходим, чтобы не создавать кнопку "Продолжить"
                }

                int nextIndex = index + 1;
                string btnText = (nextIndex >= _currentDialogue.Nodes.Count) ? "[Закончить разговор]" : "[Продолжить]";
                ui.SpawnChoiceButton(btnText, nextIndex, true, (targetIdx) => DisplayNode(nextIndex));
            }
        }

        private void ExecuteActions(List<DialogueAction> actions)
        {
            if (actions == null || _playerObj == null) return;

            Debug.Log($"[Dialogue] Попытка выполнить действий: {actions.Count}"); // <-- ЛОГ

            foreach (var action in actions)
            {
                if (action == null)
                {
                    Debug.LogWarning("[Dialogue] Найдено ПУСТОЕ действие (null) в списке!");
                    continue;
                }

                Debug.Log($"[Dialogue] Выполняем: {action.name}"); // <-- ЛОГ
                action.Execute(_playerObj);
            }
        }

        public void EndDialogue()
        {
            IsDialogueActive = false;
            _currentDialogue = null;
            _currentNpcFaction = null;
            ui.Hide();

            if (playerController != null) playerController.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void RefreshReferences()
        {
            ui = FindFirstObjectByType<DialogueUI>();
            playerController = FindFirstObjectByType<PlayerController>();
            Debug.Log("[DialogueRunner] Ссылки обновлены.");
        }
    }
}