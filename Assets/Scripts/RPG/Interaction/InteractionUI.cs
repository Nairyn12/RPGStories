using TMPro;
using UnityEngine;

namespace Game.RPG
{
    public class InteractionUI : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI promptText;

        private void Start()
        {
            if (interactor == null) interactor = FindFirstObjectByType<PlayerInteractor>();

            panel.SetActive(false);
            if (interactor != null) interactor.OnInteractPromptChanged += HandlePromptChanged;
        }

        private void OnDestroy()
        {
            if (interactor != null) interactor.OnInteractPromptChanged -= HandlePromptChanged;
        }

        private void HandlePromptChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                panel.SetActive(false);
            }
            else
            {
                panel.SetActive(true);
                promptText.text = $"[E] {text}";
            }
        }
    }
}