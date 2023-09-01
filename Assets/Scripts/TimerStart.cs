using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TimerStart : MonoBehaviour
{
    [SerializeField] private float _timerStart;
    [SerializeField] private TMP_Text _textTimer;
    [SerializeField] private Player _player;

    private float _timer;
    
    private void Start()
    {
        _timer = _timerStart;
        _textTimer.text = _textTimer.ToString();
        StartCoroutine(StartTime());
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
            yield return new WaitForSeconds(1f);
        }

        OnEnd();
    }

    private void OnEnd()
    {
        StopCoroutine(StartTime());
        _player.AttackSkill();
    }
}