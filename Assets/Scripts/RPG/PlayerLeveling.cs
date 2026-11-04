using System;
using UnityEngine;

namespace Game.RPG
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerLeveling : MonoBehaviour, ISaveable
    {
        [SerializeField] private XpCurveSO xpCurve;
        [SerializeField] private int currentXp = 0;
        private PlayerStats _stats;

        public event Action<int, int> OnXpChanged;
        public event Action<int> OnLevelUp;

        public int CurrentXp => currentXp;
        public int MaxLevel => xpCurve ? xpCurve.MaxLevel : 99;
        public int XpForNextLevel => xpCurve ? xpCurve.GetXpForLevel(_stats.Level) : 100;
        public float XpProgress01 => XpForNextLevel > 0 ? Mathf.Clamp01((float)currentXp / XpForNextLevel) : 0f;

        private void Awake() => _stats = GetComponent<PlayerStats>();

        public void AddXp(int amount)
        {
            if (amount <= 0 || _stats.Level >= MaxLevel) return;
            currentXp += amount;
            while (_stats.Level < MaxLevel)
            {
                int required = xpCurve.GetXpForLevel(_stats.Level);
                if (currentXp < required) break;
                currentXp -= required;
                _stats.LevelUp();
                OnLevelUp?.Invoke(_stats.Level);
            }
            if (_stats.Level >= MaxLevel) currentXp = 0;
            OnXpChanged?.Invoke(currentXp, XpForNextLevel);
        }

        // ==== ISaveable ====
        public string SaveKey => "PlayerLeveling";
        public string CaptureState() => JsonUtility.ToJson(new XpData { Xp = currentXp });
        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<XpData>(json);
            if (data != null) { currentXp = Mathf.Max(0, data.Xp); OnXpChanged?.Invoke(currentXp, XpForNextLevel); }
        }
        [Serializable] private class XpData { public int Xp; }
    }
}
