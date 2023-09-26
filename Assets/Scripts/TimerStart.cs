using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Infrastructure.Factory;
using Infrastructure.Hero;
using Infrastructure.Services;
using Logic;
using Photon.Pun;
using TMPro;
using UIExtensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimerStart : MonoBehaviour, IPunObservable 
{
    [SerializeField] private float _timerStart;
    [SerializeField] private TMP_Text _textTimer;
    [SerializeField] private Button _buttonPlay;

    private IGameFactory _gameFactory;
    private Fighter _fighter;
    private float _timer;
    private List<Fighter> _fighters = new();
    public Fighter fighter1;
    public Fighter fighter2;
    private int _endRoundCount;

    public event UnityAction BothCompleted;

    private void Start()
    {
        _timer = _timerStart;
        _textTimer.text = _textTimer.ToString();
        Debug.Log("star timer");

        // foreach (Player player in PhotonNetwork.PlayerList)
        // {
        //     
        //     _heroes.Add(player);
        //     Debug.Log("Player name " + player.IsMasterClient);
        // }
        
        StartCoroutine(CreateHero());
        
        StartCoroutine(StartTime());
    }

    private IEnumerator CreateHero()
    {
        yield return new WaitForSeconds(1f);
        GetFighters();
    }

    private void GetFighters()
    {
        var photonViews = FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in photonViews)
        {
            var player = view.Owner;
            if (player != null)
            {
                var playerObject = view.gameObject;
                _fighters.Add(playerObject.GetComponent<Fighter>());
            }
        }
        fighter1 = _fighters[0];
        fighter2 = _fighters[1];

        fighter1.RoundEnded += OnRoundEnd;
        fighter2.RoundEnded += OnRoundEnd;
    }

    private void OnRoundEnd(bool isEnd)
    {
        _endRoundCount++;

        if (_endRoundCount == 2)
        {
            BothCompleted?.Invoke();
        }
    }

    public void OnEnable()
    {
        BothCompleted += StartBattle;
        //_buttonPlay.Add(StartBattlePlay);
    }

    public void OnDisable()
    {
        //_buttonPlay.Remove(StartBattlePlay);
    }

    public void StartBattle()
    {
        _timerStart = _timer;
        //_buttonPlay.Activate();
        StartCoroutine(StartTime());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(_timerStart);
        }
        else if (stream.IsReading)
        {
            _timerStart = (float)stream.ReceiveNext();
        }
    }

    private IEnumerator StartTime()
    {
        while (_timerStart >= 0)
        {
            _textTimer.text = $"{_timerStart / 60}";
            _textTimer.text = _timerStart.ToString(CultureInfo.CurrentCulture);
            _timerStart--;
            yield return new WaitForSeconds(1.1f);
        }

        OnEndTimer();
    }

    private void OnEndTimer()
    {
        StopCoroutine(StartTime());

        foreach (Fighter fighter in _fighters)
        {
            if (fighter.PlayerData != null)
                _fighter = fighter;
        }
        
        _fighter.AttackSkill();
    }    

    // private void StartBattlePlay()
    // {
    //     _buttonPlay.Deactivate();
    //     
    //     foreach (Fighter fighter in _fighters)
    //     {
    //         fighter.AttackSkill();
    //     }
    // }

    public void AnimationStop()
    {
        for (int i = 0; i < _fighters.Count; i++)
        {
            if (_fighters[i]._isRoundEnd == true & _fighters[i + 1]._isRoundEnd == true)
            {
                _fighter.OnEndBattle();
                break;
            }
        }
    }
}