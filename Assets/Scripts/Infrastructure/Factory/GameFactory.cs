using System;
using Infrastructure.AssetManagement;
using MultiPlayer;
using Photon.Pun;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;

        public GameObject Hero1 { get; private set; }
        public GameObject Hero2 { get; private set; }
        public event Action HeroCreated;
        public event Action HeroCreated1;

        public GameFactory(IAssetProvider assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
        }                
        
        public void CreateHubMenu()
        {
            _assets.Instantiate(AssetPath.HudPath);
        }
       

        public GameObject CreateHero(PlayerTypeId typeId, PlayerStaticData staticData)
        {

            if (PhotonNetwork.IsMasterClient)
            {
                Hero1 = CreatePhotonHero(typeId, staticData.Prefab.name, AssetPath.Spawner);
                HeroCreated?.Invoke();
                var hud = CreateHudBattlePlayer1();

                foreach (var data in staticData.SkillDatas)
                {
                    hud.GetComponentInChildren<SkillsPanel>().AddPlayerSkills(data);
                }
                
                Hero1.GetComponent<Player>().SetPlayerData(staticData);
                Hero1.GetComponent<Player>().Construct(hud.GetComponentInChildren<SkillsPanel>(),
                    hud.GetComponentInChildren<Inventory>());
                
                return Hero1;
            }
            else
            {
                Hero2 = CreatePhotonHero(typeId, staticData.Prefab.name, AssetPath.Spawner1);
                HeroCreated1?.Invoke();
                Hero2.GetComponent<PhotonViewComponents>().enabled = true;

                //PhotonViewComponents photonView = hero.GetComponent<PhotonViewComponents>();

                var hud = CreateHudBattle();

                foreach (var data in staticData.SkillDatas)
                {
                    hud.GetComponentInChildren<SkillsPanel>().AddPlayerSkills(data);
                }
                
                Hero2.GetComponent<Player>().SetPlayerData(staticData);
                Hero2.GetComponent<Player>().Construct(hud.GetComponentInChildren<SkillsPanel>(),
                    hud.GetComponentInChildren<Inventory>());
                return Hero2;
            }

            // RegisterProgressWatchers(hero);
        }

        private GameObject CreatePhotonHero(PlayerTypeId typeId, string namePlayer, string spawnerPlayer)
        {
            PlayerStaticData heroData = _staticData.ForPlayer(typeId);
            GameObject heroPhoton = _assets.InstantiatePhoton(namePlayer, spawnerPlayer);

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