using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Infrastructure.Factory;
using Infrastructure.Services;
using Photon.Pun;
using TMPro;
using UIExtensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerStart : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _timerStart;
    [SerializeField] private TMP_Text _textTimer;
    [SerializeField] private Button _buttonPlay;


    private IGameFactory _gameFactory;
    private Player _player;
    private Player _secondPlayer;
    private float _timer;
    private List<Player> _heroes = new();

    private void Start()
    {
        _timer = _timerStart;
        _textTimer.text = _textTimer.ToString();
        StartCoroutine(StartTime());
    }

    private void Awake()
    {
        _gameFactory = AllServices.Container.Single<IGameFactory>();
        _gameFactory.HeroCreated += OnHeroCreated1;
        _gameFactory.HeroCreated1 += OnHeroCreated2;
    }

    private void OnHeroCreated1() => 
        _heroes.Add(_gameFactory.Hero1.GetComponent<Player>());

    private void OnHeroCreated2() =>
        _heroes.Add(_gameFactory.Hero2.GetComponent<Player>());

    private void OnEnable()
    {
        _buttonPlay.Add(StartBattlePlay);
    }

    private void OnDisable()
    {
        _buttonPlay.Remove(StartBattlePlay);
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

    public void StartBattle()
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

        OnEnd();
    }

    private void OnEnd()
    {
        StopCoroutine(StartTime());
        
        foreach (var currentPlayer in _heroes)
        {
            currentPlayer.AttackSkill();
        }
    }

    private void StartBattlePlay()
    {
        _buttonPlay.Deactivate();

        foreach (var currentPlayer in _heroes)
        {
            currentPlayer.AttackSkill();
        }
    }
}