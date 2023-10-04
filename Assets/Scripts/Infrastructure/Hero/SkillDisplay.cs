using UnityEngine;
using TMPro;
using Infrastructure.Hero;
using StaticData;
using Photon.Pun;

namespace Assets.Scripts.Infrastructure.Hero
{
    public class SkillDisplay : MonoBehaviour, IPunObservable
    {
        [SerializeField] private TMP_Text _skillText;
        [SerializeField] private GameObject _counterPrefab;

        private PhotonView _photonView;
        private Animator _animator;
        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            _animator = GetComponent<Animator>();
        }

        public void ShowAttack()
        {
            //SetattackText();
            _photonView.RPC(nameof(SetAttackText), RpcTarget.All);
        }

        [PunRPC]
        public void SetAttackText()
        {
            _animator.SetTrigger("Counter");
            Debug.Log("Attack");
        }            
        [PunRPC]
        public void ShowDefence()
            => _skillText.text = "Defence";
        [PunRPC]
        public void ShowCounter()
            => _skillText.text = "Counter";

        public void Desactivate()
            => gameObject.SetActive(false);

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }
    }
}
