using UnityEngine;

namespace Assets.Scripts.Fighters
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] protected FighterStaticData _fighterData;

        public FighterStaticData FighterData => _fighterData;
    }
}
