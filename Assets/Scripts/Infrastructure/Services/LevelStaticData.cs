using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        public List<PlayerSpawnerData> PlayerSpawners;
    }
}