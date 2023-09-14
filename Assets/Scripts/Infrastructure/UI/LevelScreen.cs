using UIExtensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.UI
{
    public class LevelScreen : MonoBehaviour
    {
        [SerializeField] private Button _battle;
        
        public event UnityAction BattleLoaded;

        private void OnEnable()
        {
            _battle.Add(BattleGame);
        }

        private void OnDisable()
        {
            _battle.Remove(BattleGame);
        }
        
        private void BattleGame()
            => BattleLoaded?.Invoke();
    }
}