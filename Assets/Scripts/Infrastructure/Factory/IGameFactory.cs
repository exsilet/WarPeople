using Infrastructure.Services;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        void CreateHubMenu();
        GameObject CreateHero(PlayerTypeId typeId, PlayerStaticData staticData);
        GameObject Hero1 { get; }
        GameObject Hero2 { get; }
        //void CreateSpawner(string spawnerId, Vector3 at, PlayerTypeId playerId);
    }
}