using Infrastructure.AssetManagement;
using Infrastructure.Hero;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Hero
{
    public class BotCreator : MonoBehaviour
    {
        [SerializeField] private Transform _botPosition;
        private List<Fighter> _fighters;

        private void Awake()
        {
            _fighters = Resources.LoadAll<Fighter>("Resources").ToList();
        }

        public void CreateBot()
        {
            var bot = Instantiate(GetRandomBot(), _botPosition);
        }

        private Fighter GetRandomBot()
        {
            return _fighters.OrderBy(o => Random.value).First();
        }
    }
}
