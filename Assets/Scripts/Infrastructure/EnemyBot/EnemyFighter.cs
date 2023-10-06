using System.Collections;
using Assets.Scripts.Infrastructure.Hero;
using Infrastructure.Hero;
using Photon.Pun;
using StaticData;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.EnemyBot
{
    public class EnemyFighter : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private float _stopSecond;
        [SerializeField] private float _hidePlayed;
        [SerializeField] private SkillDisplay _skillDisplay;
        
        private PlayerStaticData _playerData;
        private Inventory _inventory;
        private SkillsPanel _skillsPanel;
        private bool _isInitialized;
        private TimerStart _timer;
        private PhotonView _photonView;
        public bool _isRoundEnd;
        private int _currentSkill;
        
        public PlayerStaticData PlayerData => _playerData;
        public PhotonView PhotonView => _photonView;
        
        public event UnityAction<bool> RoundEnded;

        private void Start()
        {
            _timer = FindObjectOfType<TimerStart>();
            _photonView = GetComponent<PhotonView>();

            StartCoroutine(SkillEnemy());
        }

        public void Construct(SkillsPanel skillsPanel, Inventory inventory)
        {
            _skillsPanel = skillsPanel;
            _inventory = inventory;
        }

        public void SetPlayerData(PlayerStaticData staticData)
            => _playerData = staticData;
        
        private IEnumerator SkillEnemy()
        {
            yield return new WaitForSeconds(1f);
            AddSkillInventory();
        }

        private void AddSkillInventory()
        {
            for (int i = 0; i < _playerData.SkillDatas.Count; i++)
            {
                int random = Random.Range(0, _playerData.SkillDatas.Count);
                Debug.Log("int random " + random);
                _currentSkill += _playerData.SkillDatas[i].Count;
                Debug.Log("current skills " + _currentSkill);

                for (int j = 0; j < _currentSkill; j++)
                {
                    if (_playerData.SkillDatas[i].Count > 0)
                    {
                        _inventory.BySkills(_playerData.SkillDatas[i]);
                    }
                }
            }
        }
    }
}