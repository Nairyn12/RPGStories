using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.RPG
{
    public class ReputationManager : MonoBehaviour, ISaveable
    {
        [Serializable]
        public class FactionReputation
        {
            public FactionDataSO Faction;
            public int Value;
        }

        [SerializeField] private List<FactionReputation> _reputations = new();

        public event Action OnReputationChanged;

        public int GetReputation(FactionDataSO faction)
        {
            if (faction == null) return 0;
            var rep = _reputations.Find(r => r.Faction == faction);
            return rep != null ? rep.Value : 0;
        }

        public void ModifyReputation(FactionDataSO faction, int amount)
        {
            if (faction == null || amount == 0) return;

            var rep = _reputations.Find(r => r.Faction == faction);
            if (rep == null)
            {
                rep = new FactionReputation { Faction = faction, Value = 0 };
                _reputations.Add(rep);
            }

            rep.Value += amount;
            Debug.Log($"[Reputation] {faction.FactionName}: {rep.Value} ({amount:+0;-0})");
            OnReputationChanged?.Invoke();
        }

        // ==== ISaveable ====
        public string SaveKey => "ReputationManager";

        [Serializable]
        private class SaveData
        {
            public List<string> FactionIds = new();
            public List<int> Values = new();
        }

        public string CaptureState()
        {
            var data = new SaveData();
            foreach (var rep in _reputations)
            {
                if (rep.Faction != null)
                {
                    data.FactionIds.Add(rep.Faction.FactionId);
                    data.Values.Add(rep.Value);
                }
            }
            return JsonUtility.ToJson(data);
        }

        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<SaveData>(json);
            if (data == null) return;

            _reputations.Clear();

            // Загружаем все фракции из папки Resources
            var allFactions = Resources.LoadAll<FactionDataSO>("Factions");

            for (int i = 0; i < data.FactionIds.Count; i++)
            {
                var faction = allFactions.FirstOrDefault(f => f.FactionId == data.FactionIds[i]);
                if (faction != null)
                {
                    _reputations.Add(new FactionReputation { Faction = faction, Value = data.Values[i] });
                }
            }

            OnReputationChanged?.Invoke();
        }
    }
}