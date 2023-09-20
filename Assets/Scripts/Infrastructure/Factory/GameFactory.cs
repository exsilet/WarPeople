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

            if (PhotonNetwork.IsMasterClient)
            {
                GameObject hero = CreatePhotonHero(typeId, AssetPath.HeroPath, AssetPath.Spawner);
                var hud = CreateHudBattlePlayer1();
                hero.GetComponent<Player>().Construct(hud.GetComponentInChildren<SkillsPanel>(),
                    hud.GetComponentInChildren<Inventory>());
                
                return hero;
            }
            else
            {
                GameObject hero = CreatePhotonHero(typeId, AssetPath.SecondPlayerPath, AssetPath.Spawner1);
                var hud = CreateHudBattle();
                hero.GetComponent<Player>().Construct(hud.GetComponentInChildren<SkillsPanel>(),
                    hud.GetComponentInChildren<Inventory>());
                
                return hero;
            }

            return null;
            // RegisterProgressWatchers(hero);
        }

        private GameObject CreatePhotonHero(PlayerTypeId typeId, string namePlayer, string spawnerPlayer)
        {
            PlayerStaticData heroData = _staticData.ForPlayer(typeId);
            GameObject heroPhoton = _assets.InstantiatePhoton(namePlayer, spawnerPlayer);
            
            // Animator heroAnimation = heroPhoton.GetComponent<Animator>();
            // //heroAnimation = heroData.Animator;
            // Sprite iconPlayer = heroPhoton.GetComponent<SpriteRenderer>().sprite;
            // iconPlayer = heroData.Icon.sprite;

            return heroPhoton;
        }
        
        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath);

            return gameObject;
        }
        
        public GameObject CreateHudBattlePlayer1()
        {
            GameObject hud = _assets.Instantiate(AssetPath.HudBattlePlayer1Path);
            
            return hud;
        }

        public GameObject CreateHudBattle()
        {
            GameObject hud = _assets.Instantiate(AssetPath.HudBattlePlayer2Path);
            
            return hud;
        }
    }
}