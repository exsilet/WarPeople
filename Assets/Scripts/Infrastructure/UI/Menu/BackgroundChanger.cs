﻿using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.UI.Menu
{
    public class BackgroundChanger : MonoBehaviour, IPunObservable
    {
        [SerializeField] private Image _background;
        [SerializeField] private List<Sprite> _backgroundSprites;
        [SerializeField] private PhotonView _photonView;

        private int _randomNumber;

        private void Start()
        {
            _randomNumber = Random.Range(0, _backgroundSprites.Count);
            ChangeBackground();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting && PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(_randomNumber);
            }
            else if (stream.IsReading)
            {
                _randomNumber = (int)stream.ReceiveNext();
            }
        }

        public void ChangeBackground()
        {
            _photonView.RPC(nameof(SetSprite), RpcTarget.All);
        }

        [PunRPC]
        private void SetSprite()
        {
            _background.sprite = _backgroundSprites[_randomNumber];
        }

        private Sprite GetRandom()
        {
            return _backgroundSprites.OrderBy(o => Random.value).First();
        }        
    }
}
