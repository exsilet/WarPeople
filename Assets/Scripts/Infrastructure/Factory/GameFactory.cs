using System.Collections.Generic;
using System.Linq;
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

                var hud = CreateHudBattle(AssetPath.HudBattlePlayer1Path, staticData);
                
                Construct(Hero1, staticData, hud);
                
                return Hero1;
            }
            else
            {
                Hero2 = CreatePhotonHero(typeId, staticData.Prefab.name, AssetPath.Spawner1);

                Hero2.GetComponent<PhotonViewComponents>().enabled = true;
                
                var hud = CreateHudBattle(AssetPath.HudBattlePlayer2Path, staticData);

                Construct(Hero2, staticData, hud);
                
                return Hero2;
            }

            // RegisterProgressWatchers(hero);
        }
        
        public GameObject CreateHeroOffline(PlayerTypeId typeId, PlayerStaticData staticData)
        {
            Hero1 = CreatePhotonHero(typeId, staticData.Prefab.name, AssetPath.Spawner);

            var hud = CreateHudBattle(AssetPath.HudBattlePlayer1Path, staticData);

            Construct(Hero1, staticData, hud);
            return Hero1;
        }

        public GameObject CreateBot(PlayerTypeId typeId, PlayerStaticData staticData)
        {
            Hero2 = CreatePhotonHero(typeId, staticData.Prefab.name, AssetPath.Spawner1);
            Hero2.GetComponent<PhotonViewComponents>().enabled = true;
            var hud = CreateHudBattle(AssetPath.HudBattlePlayer2Path, staticData);
            Construct(Hero2, staticData, hud);
            hud.gameObject.SetActive(false);

            return Hero2;
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
            hero.GetComponent<Fighter>().SetPlayerData(staticData);
            hero.GetComponent<Fighter>().Construct(hud.GetComponentInChildren<SkillsPanel>(),
                hud.GetComponentInChildren<Inventory>());
        }
    }
}