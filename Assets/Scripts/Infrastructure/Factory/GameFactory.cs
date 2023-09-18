using Infrastructure.AssetManagement;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;

        public GameFactory(IAssetProvider assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
        }

        public void CreateHubMenu()
        {
            _assets.Instantiate(AssetPath.HudPath);
        }

        public GameObject CreateHero(PlayerTypeId typeId)
        {
            Debug.Log(typeId + " Gamefactory data player");
            
            if (PlayerManager.LocalPlayerInstance == null)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    var hero = CreatePhotonHero(typeId, AssetPath.Spawner, AssetPath.HeroPath);
                    return hero;
                }
                else
                {
                    var hero = CreatePhotonHero(typeId, AssetPath.Spawner1, AssetPath.SecondPlayerPath);
                    return hero;
                }
            }
            else
            {
                return null;
            }

            // RegisterProgressWatchers(hero);
        }

        private GameObject CreatePhotonHero(PlayerTypeId typeId, string spawnerPlayer, string namePlayer)
        {
            PlayerStaticData heroData = _staticData.ForPlayer(typeId);
            SpawnPoint spawner = InstantiateRegistered(spawnerPlayer).GetComponent<SpawnPoint>();
            spawner.Construct(this);
            return _assets.InstantiatePhoton(namePlayer, spawner.gameObject.transform.position);
        }
        
        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath);

            return gameObject;
        }

        public GameObject CreateHudBattle()
        {
            GameObject hud = _assets.Instantiate(AssetPath.HudBattlePath);

            return hud;
        }
    }
}