using Infrastructure.Factory;
using StaticData;
using UnityEngine;

namespace Infrastructure
{
    public class SpawnPoint : MonoBehaviour
    {
        //public PlayerTypeId PlayerTypeId;
        
        public string Id { get; set; }
        
        private IGameFactory _gameFactory;
        
        private bool _death;
        
        public void Construct(IGameFactory factory) =>
            _gameFactory = factory;
    }
}