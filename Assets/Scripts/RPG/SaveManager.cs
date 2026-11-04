using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.RPG
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
        private string SavePath => Path.Combine(Application.persistentDataPath, "save_slot_1.json");

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SaveGame()
        {
            // ЗАМЕНЕНО: FindFirstObjectByType вместо устаревшего FindObjectOfType
            var player = FindFirstObjectByType<PlayerStats>();
            if (player == null) { Debug.LogError("Игрок не найден!"); return; }

            GameSaveData data = new GameSaveData
            {
                SceneName = SceneManager.GetActiveScene().name,
                PlayerPosition = player.transform.position,
                PlayerRotation = player.transform.rotation,
                SaveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                PlayerLevel = player.Level
            };

            MonoBehaviour[] allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var script in allScripts)
            {
                if (script is ISaveable saveable)
                {
                    data.Entries.Add(new SaveEntry { Key = saveable.SaveKey, Json = saveable.CaptureState() });
                }
            }

            File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
            Debug.Log($"[SaveManager] Сохранено. Объектов: {data.Entries.Count}");
        }

        public void LoadGame()
        {
            if (!File.Exists(SavePath)) { Debug.LogWarning("Сейв не найден!"); return; }
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(File.ReadAllText(SavePath));
            if (data != null) StartCoroutine(LoadSceneAndApplyData(data));
        }

        private IEnumerator LoadSceneAndApplyData(GameSaveData data)
        {
            yield return SceneManager.LoadSceneAsync(data.SceneName);
            yield return null;

            // ЗАМЕНЕНО: FindFirstObjectByType вместо устаревшего FindObjectOfType
            var player = FindFirstObjectByType<PlayerStats>();
            if (player != null)
            {
                var rb = player.GetComponent<Rigidbody>();
                if (rb != null) { rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }
                player.transform.SetPositionAndRotation(data.PlayerPosition, data.PlayerRotation);
            }

            MonoBehaviour[] allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var script in allScripts)
            {
                if (script is ISaveable saveable)
                {
                    var entry = data.Entries.Find(e => e.Key == saveable.SaveKey);
                    if (entry != null) saveable.RestoreState(entry.Json);
                }
            }
            Debug.Log("[SaveManager] Игра загружена.");
        }
    }
}
