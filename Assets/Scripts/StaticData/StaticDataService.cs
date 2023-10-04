using System.Collections.Generic;
using System.Linq;
using Infrastructure.Hero;
using Infrastructure.Services;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataTowerPath = "StaticData/Player";

        private Dictionary<PlayerTypeId, PlayerStaticData> _playerStatic;
        private Dictionary<Fighter, PlayerTypeId> _fighters;
        private Dictionary<string, LevelStaticData> _level;

        public void Load()
        {
            _playerStatic = Resources
                .LoadAll<PlayerStaticData>(StaticDataTowerPath)
                .ToDictionary(x => x.PlayerTypeId, x => x);           
        }
        
        public PlayerStaticData ForPlayer(PlayerTypeId typeID) =>
            _playerStatic.TryGetValue(typeID, out PlayerStaticData staticData) 
                ? staticData 
                : null;
        
        public LevelStaticData ForLevel(string sceneKey) =>
            _level.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;
    }
}