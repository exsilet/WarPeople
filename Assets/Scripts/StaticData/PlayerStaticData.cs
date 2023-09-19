using UnityEngine;
using UnityEngine.UIElements;

namespace StaticData
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "StaticData/PlayerID")]
    public class PlayerStaticData : ScriptableObject
    {
        public PlayerTypeId PlayerTypeId;
        public Animator Animator;
        public Image Icon;
    }
}