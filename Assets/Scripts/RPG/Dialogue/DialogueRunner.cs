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

        public void RefreshReferences()
        {
            ui = FindFirstObjectByType<DialogueUI>();
            playerController = FindFirstObjectByType<PlayerController>();
            Debug.Log("[DialogueRunner] Ссылки обновлены.");
        }

        public void StartDialogue(DialogueSO dialogue, GameObject player, FactionDataSO npcFaction)
        {
            if (dialogue == null || dialogue.Nodes == null || dialogue.Nodes.Count == 0) return;

            IsDialogueActive = true;
            _playerObj = player;
            _currentDialogue = dialogue;
            _currentNpcFaction = npcFaction;

            if (playerController != null) playerController.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ui.Show(dialogue);

            _currentNodeIndex = FindStartNodeIndex();
            if (_currentNodeIndex == -1) { EndDialogue(); return; }

            DisplayNode(_currentNodeIndex);
        }

        private int FindStartNodeIndex()
        {
            if (_currentDialogue == null || _currentDialogue.Nodes == null || _currentDialogue.Nodes.Count == 0) return -1;

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
                        if (condition == null) continue;
                        if (!condition.IsMet(stats, inv, _currentNpcFaction)) { isAvailable = false; break; }
                    }
                }
                if (isAvailable) return i;
            }
            return -1;
        }

        private void DisplayNode(int index)
        {
            if (index < 0 || index >= _currentDialogue.Nodes.Count) { EndDialogue(); return; }

            _currentNodeIndex = index;
            var node = _currentDialogue.Nodes[index];
            if (node == null) { EndDialogue(); return; }

            ExecuteActions(node.OnEnterActions);
            ui.ClearChoices();
            ui.SetText(node.NpcText);

            var stats = _playerObj != null ? _playerObj.GetComponent<PlayerStats>() : null;
            if (stats == null) stats = FindFirstObjectByType<PlayerStats>();
            var inv = _playerObj != null ? _playerObj.GetComponent<PlayerInventory>() : null;
            if (inv == null) inv = FindFirstObjectByType<PlayerInventory>();

            if (node.Choices != null && node.Choices.Count > 0)
            {
                foreach (var choice in node.Choices)
                {
                    if (choice == null) continue;

                    bool isAvailable = true;
                    if (choice.Conditions != null)
                    {
                        foreach (var condition in choice.Conditions)
                        {
                            if (condition == null) continue;
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
                if (node.EndDialogueHere)
                {
                    ui.SpawnChoiceButton("[Закончить разговор]", -1, true, (targetIdx) => EndDialogue());
                    return;
                }
                int nextIndex = index + 1;
                string btnText = (nextIndex >= _currentDialogue.Nodes.Count) ? "[Закончить разговор]" : "[Продолжить]";
                ui.SpawnChoiceButton(btnText, nextIndex, true, (targetIdx) => DisplayNode(nextIndex));
            }
        }

        private void ExecuteActions(List<DialogueAction> actions)
        {
            if (actions == null || _playerObj == null) return;
            foreach (var action in actions)
            {
                if (action != null) action.Execute(_playerObj);
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
    }
}