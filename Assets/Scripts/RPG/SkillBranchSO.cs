using System.Collections.Generic;
using UnityEngine;

namespace Game.RPG
{
    [CreateAssetMenu(fileName = "Branch_", menuName = "RPG/Skill Branch")]
    public class SkillBranchSO : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Icon;
        public List<SkillDefinitionSO> Skills = new();
    }
}