using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RPG
{
    [Serializable]
    public class GameSaveData
    {
        public string SceneName;
        public Vector3 PlayerPosition;
        public Quaternion PlayerRotation;
        public string SaveDate;
        public int PlayerLevel;
        public List<SaveEntry> Entries = new();
    }

    [Serializable]
    public class SaveEntry
    {
        public string Key;
        public string Json;
    }
}
