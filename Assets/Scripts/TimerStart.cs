using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Infrastructure.EnemyBot;
using Infrastructure.Factory;
using Infrastructure.Hero;
using Photon.Pun;
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
    private List<BotFighter> _enemyFighters = new();
    private Fighter _fighter1;
    private Fighter _fighter2;
    private BotFighter _fighter3;
    private int _endRoundCount;

    public event UnityAction BothCompleted;

    public void Start()
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Hero");

        foreach (GameObject player in players)
        {
            if (player.GetComponent<BotFighter>() != null)
            {
                _enemyFighters.Add(player.GetComponent<BotFighter>());
            }

            if (player != null)
            {
                _fighters.Add(player.GetComponent<Fighter>());
            }
        }

        if (_enemyFighters.Count == 0)
        {
            _fighter1 = _fighters[0];
            _fighter2 = _fighters[1];
            
            _fighter1.RoundEnded += OnRoundEnd;
            _fighter2.RoundEnded += OnRoundEnd;
        }
        else
        {
            _fighter1 = _fighters[1];
            _fighter3 = _enemyFighters[0];
            
            _fighter1.RoundEnded += OnRoundEnd;
            _fighter3.RoundEnded += OnRoundEnd;
        }
    }

    private void OnRoundEnd(bool isEnd)
    {
        _endRoundCount++;

        if (_endRoundCount == 2)
        {
            BothCompleted?.Invoke();
            _endRoundCount = 0;
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
            if (fighter != null)
            {
                if (fighter.PlayerData != null)
                    _fighter = fighter;
            }
        }


        if (_enemyFighters.Count == 0)
        {
            _fighter.AttackSkill();
        }
        else
        {
            _fighter.AttackSkill();
            _fighter3.AttackSkill();
        }
    }    

    // private void StartBattlePlay()
    // {
    //     _buttonPlay.Deactivate();
    //     
    // }
}