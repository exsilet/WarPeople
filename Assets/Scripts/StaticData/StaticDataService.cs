using System.Collections.Generic;
using System.Linq;
using Infrastructure.Hero;
using Infrastructure.Services;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataHeroPath = "StaticData/Player";
        private const string StaticDataEnemyPath = "StaticData/Enemy";

        private Dictionary<PlayerTypeId, PlayerStaticData> _playerStatic;
        private Dictionary<PlayerTypeId, PlayerStaticData> _enemyStatic;
        private Dictionary<string, LevelStaticData> _level;

        public void Load()
        {
            _playerStatic = Resources
                .LoadAll<PlayerStaticData>(StaticDataHeroPath)
                .ToDictionary(x => x.PlayerTypeId, x => x);
            
            _enemyStatic = Resources
                .LoadAll<PlayerStaticData>(StaticDataEnemyPath)
                .ToDictionary(x => x.PlayerTypeId, x => x);
        }
        
        public PlayerStaticData ForPlayer(PlayerTypeId typeID) =>
            _playerStatic.TryGetValue(typeID, out PlayerStaticData staticData) 
                ? staticData 
                : null;
        
        public PlayerStaticData ForEnemy(PlayerTypeId typeID) =>
            _enemyStatic.TryGetValue(typeID, out PlayerStaticData staticData) 
                ? staticData 
                : null;
        
        public LevelStaticData ForLevel(string sceneKey) =>
            _level.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;
    }
}