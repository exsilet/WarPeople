﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Infrastructure.Factory;
using Infrastructure.Hero;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
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
    private Fighter _fighter1;
    private Fighter _fighter2;
    private int _endRoundCount;

    public event UnityAction BothCompleted;

    private void Start()
    {
        _timer = _timerStart;
        _textTimer.text = _textTimer.ToString();
        Debug.Log("star timer");

        StartCoroutine(CreateHero());
        
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

    private IEnumerator CreateHero()
    {
        yield return new WaitForSeconds(1f);
        GetFighters();
    }

    private void GetFighters()
    {
        var players = GameObject.FindGameObjectsWithTag("Hero");

        foreach (GameObject player in players)
        {
            if (player != null)
            {
                _fighters.Add(player.GetComponent<Fighter>());
            }
        }

        _fighter1 = _fighters[0];
        _fighter2 = _fighters[1];

        _fighter1.RoundEnded += OnRoundEnd;
        _fighter2.RoundEnded += OnRoundEnd;
    }

    private void OnRoundEnd(bool isEnd)
    {
        _endRoundCount++;

        if (_endRoundCount == 2)
        {
            BothCompleted?.Invoke();
        }
    }

    private void OnEnable()
    {
        BothCompleted += StartBattle;
        //_buttonPlay.Add(StartBattlePlay);
    }

    private void StartBattle()
    {
        _timerStart = _timer;
        //_buttonPlay.Activate();
        StartCoroutine(StartTime());
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
    // }
}