using UnityEngine;

namespace Game.RPG
{
    public class LockedObject : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private int lockDifficulty = 3;
        [SerializeField] private bool isLocked = true;

        [Header("Visuals")]
        [SerializeField] private GameObject lockedVisual;
        [SerializeField] private GameObject unlockedVisual;

        private Chest _chest;

        private void Awake()
        {
            _chest = GetComponent<Chest>();
        }

        public bool IsLocked() => isLocked;

        public string GetInteractionText()
        {
            return isLocked ? $"Взломать замок (Сложность: {lockDifficulty})" : "Открыть";
        }

        public void Interact(GameObject interactor)
        {
            Debug.Log($"[LockedObject] Попытка взаимодействия с {gameObject.name}. Заперт: {isLocked}");

            if (!isLocked)
            {
                if (_chest != null) _chest.Interact(interactor);
                return;
            }

            var stats = interactor.GetComponent<PlayerStats>() ?? FindFirstObjectByType<PlayerStats>();

            // 🛠 ВРЕМЕННО УБРАЛИ ПРОВЕРКУ НАВЫКА ДЛЯ ТЕСТА
            // float skill = stats != null ? stats.GetStat(StatType.Stealth) : 0;
            // if (skill < lockDifficulty) { ... return; }

            LockpickingManager.Instance.StartLockpick(lockDifficulty, stats, (success) =>
            {
                if (success)
                {
                    isLocked = false;
                    if (lockedVisual != null) lockedVisual.SetActive(false);
                    if (unlockedVisual != null) unlockedVisual.SetActive(true);
                    Debug.Log("[LockedObject] Успешно взломано!");

                    if (_chest != null) _chest.Interact(interactor);
                }
                else
                {
                    Debug.Log("[LockedObject] Неудача. Отмычка сломалась.");
                }
            });
        }
    }
}