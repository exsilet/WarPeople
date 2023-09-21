using Infrastructure.Services;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        void CreateHubMenu();
        GameObject CreateHero(PlayerTypeId typeId, PlayerStaticData staticData);
        GameObject CreateHudBattle();
        GameObject CreateHudBattlePlayer1();
        //void CreateSpawner(string spawnerId, Vector3 at, PlayerTypeId playerId);
    }
}