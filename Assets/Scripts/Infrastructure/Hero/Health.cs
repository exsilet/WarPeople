using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Hero
{
    [RequireComponent(typeof(PlayerAnimator))]
    public class Health : MonoBehaviour
    {
        [SerializeField] private HealthView _healthView;
        
        private int _currentHp;
        private readonly int _maxHp = 10;

        private PlayerAnimator _animator;
        private PhotonView _photonView;

        public event UnityAction<int> HealthChanged;
        public event UnityAction Died;

        public int CurrentHp => _currentHp;

        public int MaxHp => _maxHp;

        private void Start()
        {
            _animator = GetComponent<PlayerAnimator>();
            _photonView = GetComponent<PhotonView>();
            _currentHp = _maxHp;
        }

        private void OnEnable()
        {
            _photonView = GetComponent<PhotonView>();
            
            if (_photonView.IsMine)
                _photonView.RPC(nameof(DisplayMaxHero), RpcTarget.All);
        }

        [PunRPC]
        public void DisplayMaxHero()
        {
            _healthView.IconHealth();
        }

        public void ApplyDamage(int damage)
        {
            _photonView.RPC(nameof(ApplyDamageRPC), RpcTarget.All, damage);
        }

        [PunRPC]
        private void ApplyDamageRPC(int damage)
        {
            Debug.Log("yron hero");
            
            if (_photonView.IsMine == false)
                return;

            if (_photonView.IsMine && _currentHp > 0f)
            {
                _currentHp -= damage;
                _photonView.RPC(nameof(DisplayHero), RpcTarget.All, _currentHp);
            }

            if (_currentHp <= 0)
                Die();
        }

        [PunRPC]
        public void DisplayHero(int currentHp)
        {
            _healthView.DrawingLives(currentHp);
        }

        private void Die()
        {
            //_animator.PlayDeath();
            Died?.Invoke();
            Destroy(gameObject);
        }
    }
}