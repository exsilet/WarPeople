using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "StaticData/Fighter")]

    public class FighterStaticData : ScriptableObject
    {
        public Sprite FighterIcon;
        public List<SkillStaticData> SkillDatas;       
        
        //public Sprite FighterIcon => _fighterIcon;
    }
}
