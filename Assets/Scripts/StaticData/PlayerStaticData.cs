using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "StaticData/PlayerID")]
    public class PlayerStaticData : ScriptableObject
    {
        public Sprite Icon;
        public GameObject Prefab;
        public PlayerTypeId PlayerTypeId;
        public List<SkillStaticData> SkillDatas;
    }
}