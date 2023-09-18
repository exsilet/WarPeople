using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "StaticData/PlayerID")]
    public class PlayerStaticData : ScriptableObject
    {
        public PlayerTypeId PlayerTypeId;
    }
}