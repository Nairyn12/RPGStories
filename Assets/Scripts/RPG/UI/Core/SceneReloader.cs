using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.RPG
{
    public class SceneReloader : MonoBehaviour
    {
        private void OnEnable()
        {
            // Подписываемся на событие загрузки любой сцены
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[SceneReloader] Сцена {scene.name} загружена. Принудительно обновляем ссылки...");

            // 1. Ищем DialogueRunner и обновляем ему ссылки
            var dialogueRunner = FindFirstObjectByType<DialogueRunner>();
            if (dialogueRunner != null)
            {
                dialogueRunner.RefreshReferences();
            }

            // 2. Ищем UI и принудительно обновляем
            var inventoryUI = FindFirstObjectByType<InventoryUI>();
            if (inventoryUI != null) inventoryUI.ForceUpdateUI();

            var equipmentSlots = FindObjectsByType<EquipmentSlotUI>(FindObjectsSortMode.None);
            foreach (var eq in equipmentSlots) eq.UpdateUI();

            var quickSlots = FindObjectsByType<QuickSlotUI>(FindObjectsSortMode.None);
            foreach (var qs in quickSlots) qs.UpdateUI();
        }
    }
}