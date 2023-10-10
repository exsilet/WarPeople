using Photon.Pun;
using UnityEngine;

namespace MultiPlayer
{
    public class PhotonViewComponents : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;

        public void OnEnable()
        {
            _photonView.RPC(nameof(FlipRPS), RpcTarget.All);            
        }

        [PunRPC]
        private void FlipRPS()
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}