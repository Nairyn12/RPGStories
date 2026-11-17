using UnityEngine;

namespace Game.RPG
{
    public class DeathScreen : MonoBehaviour
    {
        [SerializeField] private GameObject screenPanel;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private SaveManager saveManager;

        private void Start()
        {
            if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth>();
            if (saveManager == null) saveManager = FindFirstObjectByType<SaveManager>();

            if (playerHealth != null) playerHealth.OnDeath += ShowScreen;
        }

        private void OnDestroy()
        {
            if (playerHealth != null)
                playerHealth.OnDeath -= ShowScreen;
        }

        private void ShowScreen()
        {
            if (screenPanel != null) screenPanel.SetActive(true);
            Time.timeScale = 0f; // Останавливаем время
            Cursor.lockState = CursorLockMode.None; // Освобождаем мышку
            Cursor.visible = true;
        }

        public void OnLoadButtonClicked()
        {
            Time.timeScale = 1f; // Возвращаем время
            if (saveManager != null) saveManager.LoadGame();
            else Debug.Log("SaveManager не назначен!");
        }
    }
}
