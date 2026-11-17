using UnityEngine;

namespace Game.RPG
{
    public abstract class DialogueAction : ScriptableObject
    {
        public abstract void Execute(GameObject player);
    }
}