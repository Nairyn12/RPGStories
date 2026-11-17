using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.RPG
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Main Panel")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Choices")]
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private GameObject choiceButtonPrefab;

        private List<GameObject> _spawnedButtons = new();

        public void Show(DialogueSO dialogue)
        {
            dialoguePanel.SetActive(true);
            nameText.text = dialogue.NpcName;
        }

        public void Hide()
        {
            dialoguePanel.SetActive(false);
            ClearChoices();
        }

        public void SetText(string text) => dialogueText.text = text;

        public void ClearChoices()
        {
            foreach (var btn in _spawnedButtons) Destroy(btn);
            _spawnedButtons.Clear();
        }

        // Обновили метод: добавили isAvailable и Action<int> для передачи индекса
        public void SpawnChoiceButton(string text, int targetIndex, bool isAvailable, System.Action<int> onChoiceSelected)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choicesContainer);
            var tmp = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = text;

            Button btn = btnObj.GetComponent<Button>();
            btn.interactable = isAvailable; // Выключаем кнопку, если условие не пройдено

            if (!isAvailable)
            {
                tmp.color = Color.gray; // Красим в серый, если навыка не хватает
            }

            btn.onClick.AddListener(() => {
                onChoiceClicked(targetIndex, onChoiceSelected);
            });

            _spawnedButtons.Add(btnObj);
        }

        private void onChoiceClicked(int targetIndex, System.Action<int> callback)
        {
            callback?.Invoke(targetIndex);
        }
    }
}