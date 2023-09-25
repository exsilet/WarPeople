using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Infrastructure.Factory;
using Infrastructure.Services;
using MultiPlayer;
using Photon.Pun;
using TMPro;
using UIExtensions;
using UnityEngine;
using UnityEngine.UI;
using Player = Infrastructure.Hero.Player;

public class TimerStart : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _timerStart;
    [SerializeField] private TMP_Text _textTimer;
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private BattleActivated _activated;

    private IGameFactory _gameFactory;
    private float _timer;
    private List<Player> _heroes = new();

    private void Start()
    {
        _timer = _timerStart;
        _textTimer.text = _textTimer.ToString();
        Debug.Log("star timer");
        //StartCoroutine(StartTime());
    }

    private void Awake()
    {
        _gameFactory = AllServices.Container.Single<IGameFactory>();
        _gameFactory.HeroCreated += OnHeroCreated1;
        Debug.Log("awake time");
    }

    private void OnEnable()
    {
        _buttonPlay.Add(StartBattlePlay);
        _gameFactory.HeroCreated1 += OnHeroCreated2;
        Debug.Log("onEnable time");
    }

    private void OnDisable()
    {
        _buttonPlay.Remove(StartBattlePlay);
    }

    public void StartBattle()
    {
        _timerStart = _timer;
        _buttonPlay.Activate();
        //StartCoroutine(StartTime());
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

    private void OnHeroCreated1()
    {
        // _heroes.Add(_gameFactory.Hero1.GetComponent<Player>());
        // _heroes.Add(_gameFactory.Hero2.GetComponent<Player>());
        PhotonView hero = FindObjectOfType<PhotonView>();
        _heroes.Add(hero.GetComponent<Player>());
        // _heroes.Add(hero.GetComponent<Player>());
        Debug.Log("crated hero to scene");
    }

    private void OnHeroCreated2()
    {
        _heroes.Add(_gameFactory.Hero2.GetComponent<Player>());
        Debug.Log("crated hero2 to scene");
        
        foreach (var currentPlayer in _heroes)
        {
            _activated.AddHero(currentPlayer);
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