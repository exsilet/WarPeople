using System;
using Infrastructure.AssetManagement;
using Infrastructure.Hero;
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
                
                var hud = CreateHudBattle(AssetPath.HudBattlePlayer1Path, staticData);
                
                Construct(Hero1, staticData, hud);
                
                return Hero1;
            }
            else
            {
                Hero2 = CreatePhotonHero(typeId, staticData.Prefab.name, AssetPath.Spawner1);
                HeroCreated?.Invoke();
                
                Hero2.GetComponent<PhotonViewComponents>().enabled = true;
                
                var hud = CreateHudBattle(AssetPath.HudBattlePlayer2Path, staticData);

                Construct(Hero2, staticData, hud);
                
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

        private GameObject CreateHudBattle(string path, PlayerStaticData staticData)
        {
            GameObject hud = _assets.Instantiate(path);
            
            foreach (var data in staticData.SkillDatas)
            {
                hud.GetComponentInChildren<SkillsPanel>().AddPlayerSkills(data);
            }
            
            return hud;
        }

        private void Construct(GameObject hero, PlayerStaticData staticData, GameObject hud)
        {
            hero.GetComponent<Player>().SetPlayerData(staticData);
            hero.GetComponent<Player>().Construct(hud.GetComponentInChildren<SkillsPanel>(),
                hud.GetComponentInChildren<Inventory>());
        }
    }
}