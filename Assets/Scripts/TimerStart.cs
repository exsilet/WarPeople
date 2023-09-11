using System.Collections;
using System.Globalization;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class TimerStart : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _timerStart;
    [SerializeField] private TMP_Text _textTimer;
    [SerializeField] private Player _player;
    [SerializeField] private Player _secondPlayer;

    private float _timer;
    
    private void Start()
    {
        _timer = _timerStart;
        _textTimer.text = _textTimer.ToString();
        //StartCoroutine(StartTime());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(_timerStart);
        }else 
        if (stream.IsReading)
        {
            _timerStart = (float)stream.ReceiveNext();
        }
    }

    public void StartBattle()
    {
        _timerStart = _timer;
        StartCoroutine(StartTime());
    }

    private IEnumerator StartTime()
    {
        while (_timerStart >= 0)
        {
            _textTimer.text = $"{_timerStart/60}";
            _textTimer.text = _timerStart.ToString(CultureInfo.CurrentCulture);
            _timerStart--;
            yield return new WaitForSeconds(1.1f);
        }

        OnEnd();
    }

    private void OnEnd()
    {
        StopCoroutine(StartTime());
        _player.AttackSkill();
        _secondPlayer.AttackSkill();
    }
}